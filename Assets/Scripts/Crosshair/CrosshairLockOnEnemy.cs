using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairLockOnEnemy : MonoBehaviour
{
    private Transform _player;
    private MoveCrosshair _moveCrosshairScript;

    private List<Enemy> _enemiesWithinCrosshair = new List<Enemy>();
    private Enemy _currentEnemyLocked;

    [SerializeField] private float _angleLockWeight = 0.7f;
    private float _distanceLockWeight => 1f - _angleLockWeight;

    private bool _wasPlayerLockedOnEnemyLastFrame;

    //[SerializeField] private float _stickMagnitudeToSwitch = 0.2f;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _moveCrosshairScript = GetComponent<MoveCrosshair>();
    }

    private void FixedUpdate()
    {
        
        if (InputManager.isPlayerLockedOnEnemy && !_wasPlayerLockedOnEnemyLastFrame)
            ActivateEnemyLock();

        _wasPlayerLockedOnEnemyLastFrame = InputManager.isPlayerLockedOnEnemy;

        
        FollowLockedEnemy();
        StopEnemyLockOnKill(_currentEnemyLocked);
        
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Enemy enemy))
        {
            if (!_enemiesWithinCrosshair.Contains(enemy))
            {
                _enemiesWithinCrosshair.Add(enemy);
                Debug.Log(_enemiesWithinCrosshair.ToString());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && _enemiesWithinCrosshair.Contains(enemy))
        {
            _enemiesWithinCrosshair.Remove(enemy);
            Debug.Log(_enemiesWithinCrosshair.ToString());
        }
    }

    private void ActivateEnemyLock()
    {
        if (InputManager.isPlayerLockedOnEnemy)
        {
            if (_enemiesWithinCrosshair.Count <= 0)
            {
                InputManager.isPlayerLockedOnEnemy = false;
                _currentEnemyLocked = null;
                return;
            }

            Enemy lowestScoreEnemy = null;
            float lowestScore = 1000f;
            foreach (Enemy enemy in _enemiesWithinCrosshair)
            {
                //On calcule l'ennemi le plus proche en rotation
                float enemyAngleFromPlayer = Mathf.Atan2(
                    enemy.transform.position.x -  _player.transform.position.x, 
                    enemy.transform.position.y - _player.transform.position.y
                ) * Mathf.Rad2Deg;

                float angleBetweenPlayerAndEnemy = Mathf.DeltaAngle(transform.eulerAngles.z, enemyAngleFromPlayer);

                //On calcule ensuite l'ennemi le plus proche en distance

                float enemyDistanceFromCrosshair = Vector2.Distance(transform.position, enemy.transform.position);

                //On finit en calculant le score et en séléctionnant l'ennemi à lock

                float score = ((angleBetweenPlayerAndEnemy / 180) * _angleLockWeight) + ((enemyDistanceFromCrosshair / _moveCrosshairScript.crosshairDistance) * _distanceLockWeight);
                if (score < lowestScore)
                {
                    lowestScore = score;
                    lowestScoreEnemy = enemy;
                }
            }
            if (lowestScoreEnemy != null)
            {
                SetLockedEnemy(lowestScoreEnemy);
            }
            
        }
    }

    private void SetLockedEnemy(Enemy enemy)
    {
        if (enemy == null || !InputManager.isPlayerLockedOnEnemy)
            return;
        _currentEnemyLocked = enemy;
    }

    private void FollowLockedEnemy()
    {
        if (_currentEnemyLocked == null)
        {
            InputManager.isPlayerLockedOnEnemy = false;
            return;
        }
        if (InputManager.isPlayerLockedOnEnemy)
        {
            //transform.position = _currentEnemyLocked.transform.position;
            transform.position = Vector2.Lerp(
                transform.position,
                _currentEnemyLocked.transform.position,
                _moveCrosshairScript.crosshairSmoothingTime * Time.fixedDeltaTime
            );
        }
        else
        {
            _currentEnemyLocked = null;
        }
    }

    private void StopEnemyLockOnKill(Enemy enemy)
    {
        if (enemy == null)
        {
            InputManager.isPlayerLockedOnEnemy = false;
        }
    }

    //private void SwitchEnemyLock(EnemyBase enemyLocked, List<EnemyBase> enemyList)
    //{
    //    if (enemyList.Count < 2)
    //        return;
    //    if (!InputManager.isPlayerLockedOnEnemy)
    //        return;
    //    if (enemyLocked == null)
    //        return;

    //    //Etablir la liste des ennemis présent dans la crosshair (enemyLocked exclu)
    //    List<EnemyBase> potentialEnemiesList = enemyList;
    //    potentialEnemiesList.Remove(enemyLocked);

    //    if (InputManager.isGamepad)
    //    {
    //        if (InputManager.rightStickDirection.magnitude < _stickMagnitudeToSwitch)
    //            return;

    //        EnemyBase lowestScoreEnemy = null;
    //        float lowestScore = 1000f;
    //        foreach (EnemyBase enemy in potentialEnemiesList)
    //        {
    //            float rightStickAngle = Mathf.Atan2(
    //                InputManager.rightStickDirection.x,
    //                InputManager.rightStickDirection.y
    //            ) * Mathf.Rad2Deg;

    //            //On calcule l'ennemi le plus proche en rotation
    //            float enemyAngleFromCrosshair = Mathf.Atan2(
    //                enemy.transform.position.x - transform.position.x,
    //                enemy.transform.position.y - transform.position.y
    //            ) * Mathf.Rad2Deg;

    //            float angleBetweenStickAndEnemy = Mathf.Abs(Mathf.DeltaAngle(rightStickAngle, enemyAngleFromCrosshair));

    //            //On calcule ensuite l'ennemi le plus proche en distance

    //            float enemyDistanceFromCrosshair = Vector2.Distance(transform.position, enemy.transform.position);

    //            //On finit en calculant le score et en séléctionnant l'ennemi à lock

    //            float score = ((angleBetweenStickAndEnemy / 180) * _angleLockWeight) + ((enemyDistanceFromCrosshair / _moveCrosshairScript.crosshairDistance) * _distanceLockWeight);
    //            if (score < lowestScore)
    //            {
    //                lowestScore = score;
    //                lowestScoreEnemy = enemy;
    //            }
    //        }
    //        if (lowestScoreEnemy != null)
    //        {
    //            SetLockedEnemy(lowestScoreEnemy);
    //        }
    //    }
    //}
}
