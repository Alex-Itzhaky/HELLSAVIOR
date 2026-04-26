using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairLockOnEnemy : MonoBehaviour
{
    private Transform player;
    private MoveCrosshair moveCrosshairScript;

    private List<EnemyBase> EnemiesWithinCrosshair = new List<EnemyBase>();
    private EnemyBase currentEnemyLocked;

    [SerializeField] private float angleLockWeight = 0.7f;
    private float distanceLockWeight => 1f - angleLockWeight;

    private bool wasPlayerLockedOnEnemyLastFrame;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        moveCrosshairScript = GetComponent<MoveCrosshair>();
    }

    private void Update()
    {
        
        if (InputManager.isPlayerLockedOnEnemy && !wasPlayerLockedOnEnemyLastFrame)
            ActivateEnemyLock();

        wasPlayerLockedOnEnemyLastFrame = InputManager.isPlayerLockedOnEnemy;

        
        FollowLockedEnemy();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyBase enemy))
        {
            if (!EnemiesWithinCrosshair.Contains(enemy))
            {
                EnemiesWithinCrosshair.Add(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyBase enemy = collision.GetComponent<EnemyBase>();
        if (enemy != null && EnemiesWithinCrosshair.Contains(enemy))
        {
            EnemiesWithinCrosshair.Remove(enemy);
        }
    }

    private void ActivateEnemyLock()
    {
        if (InputManager.isPlayerLockedOnEnemy)
        {
            if (EnemiesWithinCrosshair.Count <= 0)
            {
                InputManager.isPlayerLockedOnEnemy = false;
                currentEnemyLocked = null;
                return;
            }

            EnemyBase lowestScoreEnemy = null;
            float lowestScore = 1000f;
            foreach (EnemyBase enemy in EnemiesWithinCrosshair)
            {
                //On calcule l'ennemi le plus proche en rotation
                float enemyAngleFromPlayer = Mathf.Atan2(
                    enemy.transform.position.x -  player.transform.position.x, 
                    enemy.transform.position.y - player.transform.position.y
                ) * Mathf.Rad2Deg;

                float angleBetweenPlayerAndEnemy = Mathf.DeltaAngle(transform.eulerAngles.z, enemyAngleFromPlayer);

                //On calcule ensuite l'ennemi le plus proche en distance

                float enemyDistanceFromCrosshair = Vector2.Distance(moveCrosshairScript.transform.position, enemy.transform.position);

                //On finit en calculant le score et en séléctionnant l'ennemi à lock

                float score = ((angleBetweenPlayerAndEnemy / 180) * angleLockWeight) + ((enemyDistanceFromCrosshair / moveCrosshairScript.crosshairDistance) * distanceLockWeight);
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

    private void SetLockedEnemy(EnemyBase enemy)
    {
        if (enemy == null || !InputManager.isPlayerLockedOnEnemy)
            return;
        currentEnemyLocked = enemy;
        Debug.Log(currentEnemyLocked.name);
    }

    private void FollowLockedEnemy()
    {
        if (InputManager.isPlayerLockedOnEnemy)
        {
            if (currentEnemyLocked == null)
                return;
            transform.position = currentEnemyLocked.transform.position;
        }
        else
        {
            currentEnemyLocked = null;
        }
    }
}
