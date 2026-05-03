using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public int currentHp;
    [SerializeField] protected EnemyState currentEnemyState;
    [SerializeField] private Rigidbody2D _rb;
    private HealthController _healthController;
    protected Transform playerTransform;
    protected bool canEnemyAttack;
    private bool _isDamageable = true;
    protected bool isSpawning = false;
    [SerializeField] private float _spawnTime = 1f;
    [SerializeField] protected EnemyData enemyData;
    private Vector2 _targetDirection;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _healthController = playerTransform.GetComponent<HealthController>();
    }
    private void Start()
    {
        InitEnemy();
    }
    private void FixedUpdate()
    {
        HandleEnemyStateMachine();
        SetVelocity();
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _healthController.TakeDamage(enemyData.damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _healthController.TakeDamage(enemyData.damage);
        }
    }

    private void InitEnemy()
    {
        gameObject.SetActive(true);
        currentHp = enemyData.maxHp;
        currentEnemyState = EnemyState.Spawning;
    }

    private IEnumerator AttackCooldownCoroutine(float duration)
    {
        canEnemyAttack = false;
        yield return new WaitForSeconds(duration);
        canEnemyAttack = true;
    }


    public void Damage(int damage)
    {
        if (!_isDamageable)
            return;
        currentHp -= damage;
        if (currentHp <= 0)
            currentEnemyState = EnemyState.Dead;
    }

    private bool _CanEnemyAttack()
    {
        float distanceBetweenEnemyAndPlayer = Vector2.Distance(playerTransform.position, transform.position);
        return canEnemyAttack && distanceBetweenEnemyAndPlayer <= enemyData.attackRange;
    }


    private void UpdateTargetDirection()
    {
        Vector2 enemyToPlayerVector = playerTransform.position - transform.position;
        Vector2 directionToPlayer = enemyToPlayerVector.normalized;
        _targetDirection = directionToPlayer;
    }

    private void RotateTowardsTarget()
    {
        float targetAngle = Mathf.Atan2(playerTransform.position.y - transform.position.y, playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
        Vector3 targetEulerAngles = new Vector3(0f, 0f, targetAngle);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetEulerAngles, enemyData.rotateSpeed * Time.fixedDeltaTime);
    }

    private void SetVelocity()
    {
        if (_targetDirection == null || _targetDirection == Vector2.zero || currentEnemyState != EnemyState.Chase)
        {
            _rb.linearVelocity = Vector2.zero;
        }
        else
        {
            _rb.linearVelocity = transform.up * enemyData.baseSpeed;
        }
    }


    protected virtual void HandleEnemyStateMachine()
    {
        switch (currentEnemyState)
        {
            case EnemyState.Spawning:
                if (isSpawning)
                    return;
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
        if (isSpawning)
            return;
        _rb.linearVelocity = Vector2.zero;
        StartCoroutine(SpawningCoroutine());
    }

    private IEnumerator SpawningCoroutine()
    {
        isSpawning = true;

        _rb.linearVelocity = Vector2.zero; //Immobilise
        _isDamageable = false; //Pas de damage
        GetComponent<Collider2D>().enabled = false; //Pas de collisions

        yield return new WaitForSeconds(_spawnTime);

        _isDamageable = true;
        GetComponent<Collider2D>().enabled = true;

        isSpawning = false;
    }

    protected virtual void OnIdle()
    {
        //Jouer animation idle
    }

    protected virtual void OnChase()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
    }

    protected virtual void OnPreparingAttack()
    {
        //jouer animation préparation attaque

    }

    protected virtual void OnAttacking()
    {
        //jouer animation attaque

        //Activer hitbox attaque


    }

    protected virtual void OnDamaged()
    {
        //trigger particules

        //trigger shader damaged


    }

    protected virtual void OnDead()
    {
        //Jouer anim mort

        //Une fois anim finie :
        Destroy(gameObject);
    }

    #endregion

}
