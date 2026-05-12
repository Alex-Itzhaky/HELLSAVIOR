using UnityEngine;

public class LightEnemy : Enemy
{
    protected override void HandleEnemyStateMachine()
    {
        switch(currentEnemyState)
        {
            case EnemyState.Chase:
                base.OnChase();
                break;
            case EnemyState.Idle:
                base.OnIdle();
                if (isPlayerDead)
                    currentEnemyState = EnemyState.Idle;
                else
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
