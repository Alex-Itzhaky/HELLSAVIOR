using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public enum WaveState
    {
        Start,
        Playing,
        Stopped,
        Transitioning
    }

    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private BonusSpawner _bonusSpawner;
    private WaveState _currentWaveState;

    private List<EnemyType> _enemiesToSpawn = new List<EnemyType>();
    [Header("Wave Duration Settings")]
    [SerializeField] private float _baseWaveDuration;
    [SerializeField] private float _waveDurationMultiplier;
    [SerializeField] private float _transitionTime;
    
    private float _currentWaveDuration;
    private float _elapsedWaveTime;
    public float ElapsedTimePercentage => _elapsedWaveTime / _currentWaveDuration;
    public WaveState CurrentWaveState => _currentWaveState;
    
    public int WaveCount { get; private set; }

    [Header("Wave Enemy Settings")]
    [SerializeField] private int _baseNumberOfEnemiesPerWave;
    [SerializeField] private float _lightEnemyChance;
    [SerializeField] private float _rangedEnemyChance;
    [SerializeField] private float _heavyEnemyChance;
    [SerializeField] private float _mediumEnemyChance;

    [Header("Debug")]
    [SerializeField] private int _currentNumberOfEnemiesPerWave;
    //private bool _canLightEnemySpawn = true;
    //private bool _canRangedEnemySpawn = false;
    //private bool _canHeayEnemySpawn = false;
    //private bool _canMediumEnemySpawn = false;

    private bool _isStarting = false;
    private bool _isPlaying = false;
    private bool _isTransitioning = false;
    private bool _isStopped = false;


    private void Start()
    {
        InitWaves();
    }

    private void Update()
    {
        HandleWaveStateMachine();
    }

    public void InitWaves()
    {
        _currentWaveDuration = _baseWaveDuration;
        _currentNumberOfEnemiesPerWave = _baseNumberOfEnemiesPerWave;
        _currentWaveState = WaveState.Transitioning;
        WaveCount = 1;
    }

    private void UpdateWaveDuration()
    {
        _currentWaveDuration = Mathf.Ceil(_currentWaveDuration * _waveDurationMultiplier);
    }

    private void UpdateWaveCount()
    {
        WaveCount++;
    }

    private void UpdateEnemyCountPerWave(int amount)
    {
        _currentNumberOfEnemiesPerWave += amount;
    }

    private void UpdateEnemySpawnCondition()
    {

    }


    private void HandleWaveStateMachine()
    {
        switch (_currentWaveState)
        {
            case WaveState.Start:
                if (_isStarting)
                    return;
                OnWaveStart();
                break;
            case WaveState.Playing:
                if (_isPlaying)
                    return;
                OnWavePlaying();
                break;
            case WaveState.Stopped:
                if (_isStopped)
                    return;
                OnWaveStop();
                break;
            case WaveState.Transitioning:
                OnWaveTransition();
                break;
        }
    }

    private IEnumerator ManageWaveCoroutine()
    {
        _isPlaying = true;

        _elapsedWaveTime = 0f;
        while (_elapsedWaveTime < _currentWaveDuration)
        {
            _elapsedWaveTime += Time.deltaTime;
            yield return null;
        }

        _isPlaying = false;
        UpdateWaveCount();
        UpdateWaveDuration();
        UpdateEnemyCountPerWave(1); //A remplacer lorsque les settings de chaque wave seront définis
        _currentWaveState = WaveState.Transitioning;
    }

    private void OnWaveStart()
    {
        _enemiesToSpawn = CreateWave();
        foreach (EnemyType enemy in _enemiesToSpawn)
        {
            _enemySpawner.RequestEnemySpawn(enemy);
        }
        _bonusSpawner.SpawnBonus();
        _currentWaveState = WaveState.Playing;
    }

    private List<EnemyType> CreateWave()
    {
        if (_lightEnemyChance + _rangedEnemyChance + _mediumEnemyChance + _heavyEnemyChance != 1f)
        {
            Debug.LogWarning("Les taux de spawn des ennemis ne s'additionnent pas à 1");
        }
            
        List<EnemyType> enemyList = new List<EnemyType>();
        for (int i = 0; i < _currentNumberOfEnemiesPerWave; i++)
        {
            float randomEnemyChance = Random.Range(0f, 1f);
            if (randomEnemyChance < _lightEnemyChance)
                enemyList.Add(EnemyType.Light);
            else if (randomEnemyChance < _lightEnemyChance + _rangedEnemyChance)
                enemyList.Add(EnemyType.Ranged);
            else if (randomEnemyChance < _lightEnemyChance + _rangedEnemyChance + _mediumEnemyChance)
                enemyList.Add(EnemyType.Medium);
            else if (randomEnemyChance < _lightEnemyChance + _rangedEnemyChance + _mediumEnemyChance + _heavyEnemyChance)
                enemyList.Add(EnemyType.Heavy);
            else
                enemyList.Add(EnemyType.Light);
        }

        return enemyList;
    }

    private void OnWavePlaying()
    {
        if (_isPlaying)
            return;
        StartCoroutine(ManageWaveCoroutine());
    }

    private IEnumerator TransitionWaveCoroutine()
    {
        _isTransitioning = true;
        //Afficher messages
        //Barre de progression de wave reset
        yield return new WaitForSeconds(_transitionTime);
        _isTransitioning = false;
        _currentWaveState = WaveState.Start;
    }

    private void OnWaveTransition()
    {
        if (_isTransitioning)
            return;
        StartCoroutine(TransitionWaveCoroutine());
    }

    public void OnWaveStop()
    {
        if (_isStopped)
            return;
        //Afficher messages
        _isStopped = true;
        _currentWaveState = WaveState.Stopped;
        StopAllCoroutines();
    }

    [ContextMenu("Test StartWave")]
    private void TestStartWave()
    {
        OnWaveStart();
    }

    [ContextMenu("Test InitWaves")]
    private void TestInitWaves()
    {
        InitWaves();
    }
}
