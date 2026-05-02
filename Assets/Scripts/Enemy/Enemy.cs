using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public int currentHp;
    protected EnemyState currentEnemyState;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private HealthController _healthController;
    protected Transform playerTransform;
    protected bool canEnemyAttack;
    private bool isDamageable = true;
    [SerializeField] private float spawnTime = 1f;
    [SerializeField] protected EnemyData enemyData;

    private void Awake()
    {
        playerTransform = _healthController.transform;
    }
    private void Start()
    {
        InitEnemy();
    }
    private void FixedUpdate()
    {
        
    }

    private void Update()
    {
        HandleEnemyStateMachine();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _healthController.TakeDamage(enemyData.damage);
        }
    }

    private void InitEnemy()
    {
        gameObject.SetActive(false);
        currentEnemyState = EnemyState.Spawning;
    }

    private IEnumerator AttackCooldownCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
    }

    private IEnumerator SpawnTimerCoroutine(float duration)
    {
        isDamageable = true;
        yield return new WaitForSeconds(duration);
        isDamageable = false;
    }

    public void Damage(int damage)
    {
        currentHp -= damage;
        if (currentHp > 0)
        {
            currentEnemyState = EnemyState.Damaged;
        }
        else
        {
            currentEnemyState = EnemyState.Dead;
        }
    }

    private bool _CanEnemyAttack()
    {
        return canEnemyAttack;
    }


    protected virtual void HandleEnemyStateMachine()
    {
        switch (currentEnemyState)
        {
            case EnemyState.Spawning:
                OnSpawning();
                currentEnemyState = EnemyState.Chase;
                break;
            case EnemyState.Chase:
                OnChase();
                if (_CanEnemyAttack())
                {
                    currentEnemyState = EnemyState.PreparingAttack;
                }
                break;
            case EnemyState.PreparingAttack:
                OnPreparingAttack();
                currentEnemyState = EnemyState.Attacking;
                break;
            case EnemyState.Attacking:
                OnAttacking();
                currentEnemyState = EnemyState.Idle;
                break;
            case EnemyState.Idle:
                OnIdle();
                if (_CanEnemyAttack())
                {
                    currentEnemyState = EnemyState.PreparingAttack;
                }
                else
                {
                    currentEnemyState = EnemyState.Chase;
                }
                break;
            case EnemyState.Damaged:
                OnDamaged();
                break;
            case EnemyState.Dead:
                OnDead();
                break;
        }
    }

    #region IndividualStates

    protected virtual void OnSpawning()
    {
        //Jouer les animations de spawn
        
        gameObject.SetActive(true);
        _rb.linearVelocity = Vector2.zero;

    }

    protected virtual void OnIdle()
    {

    }

    protected virtual void OnChase()
    {

    }

    protected virtual void OnPreparingAttack()
    {

    }

    protected virtual void OnAttacking()
    {

    }

    protected virtual void OnDamaged()
    {

    }

    protected virtual void OnDead()
    {

    }

    #endregion

}
