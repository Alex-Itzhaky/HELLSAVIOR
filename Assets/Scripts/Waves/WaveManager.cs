using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private enum WaveState
    {
        Start,
        Playing,
        Stopped,
        Transitioning
    }

    [SerializeField] private EnemySpawner _enemySpawner;
    private WaveState _currentWaveState;

    private List<EnemyType> _enemiesToSpawn = new List<EnemyType>();
    [Header("Wave Duration Settings")]
    [SerializeField] private float _baseWaveDuration;
    [SerializeField] private float _waveDurationMultiplier;
    
    private float _currentWaveDuration;
    private float _elapsedWaveTime;
    public float _ElapsedTimePercentage => _elapsedWaveTime / _currentWaveDuration;

    private int _waveCount = 1;

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

    public void InitWaves()
    {
        _currentWaveDuration = _baseWaveDuration;
        _currentNumberOfEnemiesPerWave = _baseNumberOfEnemiesPerWave;
    }

    private void UpdateWaveDuration()
    {
        _currentWaveDuration = Mathf.Ceil(_currentWaveDuration * _waveDurationMultiplier);
    }

    private void UpdateWaveCount()
    {
        _waveCount++;
    }

    private void UpdateEnemyCountPerWave()
    {
        _currentNumberOfEnemiesPerWave += 1;
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
        _currentWaveState = WaveState.Stopped;
    }

    private void OnWaveStart()
    {
        _enemiesToSpawn = CreateWave();
        foreach (EnemyType enemy in _enemiesToSpawn)
        {
            _enemySpawner.RequestEnemySpawn(enemy);
        }
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
            Debug.Log(randomEnemyChance);
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

    private void OnWaveTransition()
    {

    }

    private void OnWaveStop()
    {
        if (_isStopped)
            return;
        
    }

    [ContextMenu("Test StartWave")]
    private void TestStartWave()
    {
        OnWaveStart();
    }
}
