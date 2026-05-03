using UnityEngine;

public class LightEnemy : Enemy
{
    protected override void HandleEnemyStateMachine()
    {
        switch(currentEnemyState)
        {
            case EnemyState.Spawning:
                if (isSpawning)
                    return;
                base.OnSpawning();
                currentEnemyState = EnemyState.Idle;
                break;
            case EnemyState.Chase:
                base.OnChase();
                break;
            case EnemyState.Idle:
                base.OnIdle();
                currentEnemyState = EnemyState.Chase;
                break;
            case EnemyState.Dead:
                base.OnDead();
                break;
            default:
                base.OnIdle();
                currentEnemyState = EnemyState.Idle;
                break;

        }
    }
}
