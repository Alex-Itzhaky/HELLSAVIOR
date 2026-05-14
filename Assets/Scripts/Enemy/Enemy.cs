using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public int currentHp;
    [SerializeField] protected EnemyState currentEnemyState;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] private HealthController _healthController;
    [SerializeField] protected Transform playerTransform;
    private bool _isDamageable = true;
    protected bool isSpawning = false;
    protected bool isPlayerDead = false;
    [SerializeField] private float _spawnTime = 1f;
    [SerializeField] protected EnemyData enemyData;
    private Vector2 _targetDirection;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (!playerTransform.gameObject.activeInHierarchy)
            StopEnemyAI();
        _healthController = playerTransform.GetComponent<HealthController>();

    }
    private void Start()
    {
        
    }
    private void FixedUpdate()
    {
        HandleEnemyStateMachine();
        SetVelocity();
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
        this.enabled = true;
        currentHp = enemyData.maxHp;
        currentEnemyState = EnemyState.Idle;
    }


    public void Damage(int damage)
    {
        if (!_isDamageable)
            return;
        currentHp -= damage;
        if (currentHp <= 0)
            currentEnemyState = EnemyState.Dead;
    }


    protected void UpdateTargetDirection()
    {
        Vector2 enemyToPlayerVector = playerTransform.position - transform.position;
        Vector2 directionToPlayer = enemyToPlayerVector.normalized;
        _targetDirection = directionToPlayer;
    }

    protected void RotateTowardsTarget()
    {
        float targetAngle = Mathf.Atan2(playerTransform.position.y - transform.position.y, playerTransform.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
        Vector3 targetEulerAngles = new Vector3(0f, 0f, targetAngle);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetEulerAngles, enemyData.rotateSpeed * Time.fixedDeltaTime);
    }

    private void SetVelocity()
    {
        if (_targetDirection == null || _targetDirection == Vector2.zero || currentEnemyState != EnemyState.Chase)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = transform.up * enemyData.baseSpeed;
        }
    }


    protected virtual void HandleEnemyStateMachine()
    {
        return;
    }

    public void StopEnemyAI()
    {
        currentEnemyState = EnemyState.Idle;
    }

    #region IndividualStates

    protected virtual void OnSpawning()
    {
        if (isSpawning)
            return;
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(SpawningCoroutine());
    }

    private IEnumerator SpawningCoroutine()
    {
        isSpawning = true;

        rb.linearVelocity = Vector2.zero; //Immobilise
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
        //jouer animation course

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
    public void OnGameOver()
    {
        currentEnemyState = EnemyState.GameOver;
    }


    #endregion

}
