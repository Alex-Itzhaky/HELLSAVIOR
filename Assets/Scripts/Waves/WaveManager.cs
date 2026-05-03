using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private enum WaveState
    {
        Spawn,
        Playing,
        Stop,
        Idle
    }

    [SerializeField] private EnemySpawner enemySpawner;

    private List<EnemyType> _enemiesToSpawn = new List<EnemyType>();
    [Header("Wave Duration Settings")]
    [SerializeField] private float _baseWaveDuration;
    [SerializeField] private float _waveDurationMultiplier;
    private float _currentWaveDuration;
    private int _waveCount = 1;

    [Header("Wave Enemy Settings")]
    [SerializeField] private int _baseNumberOfEnemiesPerWave;
    [SerializeField] private float _lightEnemyChance;
    [SerializeField] private float _rangedEnemyChance;
    [SerializeField] private float _heavyEnemyChance;
    [SerializeField] private float _mediumEnemyChance;
    private int _currentNumberOfEnemiesPerWave;
    private bool _canLightEnemySpawn = true;
    private bool _canRangedEnemySpawn = false;
    private bool _canHeayEnemySpawn = false;
    private bool _canMediumEnemySpawn = false;

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

    private void StartNewWaves()
    {

    }

    private void ResetWaves()
    {

    }

    private void StopWaves()
    {

    }

    private void HandleWaveStateMachine()
    {

    }

    private IEnumerator ManageWaveCoroutine()
    {
        yield return null;
    }
}
