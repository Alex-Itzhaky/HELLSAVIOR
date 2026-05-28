using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamageable
{
    protected int currentHp;
    protected EnemyState currentEnemyState;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] private HealthController _healthController;
    [SerializeField] protected Transform playerTransform;
    private bool _isDamageable = true;
    protected bool isSpawning = false;
    protected bool isPlayerDead = false;
    [SerializeField] private float _spawnTime = 1f;
    [SerializeField] protected EnemyData enemyData;
    private Vector2 _targetDirection;
    public UnityEvent OnDamaged;

    [Header("Audio")]
    [SerializeField] private AudioClip[] _randomSounds;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private float _averageTimeBetweenSounds;
    [SerializeField] private float _maxTimeOffsetOfSounds;

    [Header("Particles")]
    [SerializeField] private ParticleSystem _damageParticlesPrefab;
    private ParticleSystem _damageParticlesInstance;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (!playerTransform.gameObject.activeInHierarchy)
            StopEnemyAI();
        _healthController = playerTransform.GetComponent<HealthController>();
        InitEnemy();

    }
    private void Start()
    {
        currentEnemyState = EnemyState.Idle;

        if (_randomSounds.Length > 0)
            StartCoroutine(RandomSoundCoroutine());
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
        currentHp = enemyData.maxHp;
        currentEnemyState = EnemyState.Idle;
    }


    public void Damage(int damage, Vector2 attackDirection)
    {
        if (!_isDamageable)
            return;
        SpawnDamageParticles(attackDirection);

        currentHp -= damage;
        OnDamaged.Invoke();
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

    protected virtual void OnDead()
    {
        //Jouer anim mort

        //Une fois anim finie :
        SoundManager.Instance.PlaySoundFXClip(_deathSound, transform);
        Destroy(gameObject);
    }
    public void OnGameOver()
    {
        currentEnemyState = EnemyState.GameOver;
    }

    protected void PlayRandomSound()
    {
        if (_randomSounds.Length <= 0)
            return;
        else if (_randomSounds.Length == 1)
        {
            float pitchVariance = 0.1f;
            SoundManager.Instance.PlaySoundFXClip(_randomSounds[0], transform, 1f, pitchVariance);
        }
        else
        {
            int index = Random.Range(0, _randomSounds.Length - 1);
            float pitchVariance = 0.1f;
            SoundManager.Instance.PlaySoundFXClip(_randomSounds[index], transform, 1f, pitchVariance);
        }
    }

    private IEnumerator RandomSoundCoroutine()
    {
        float timer = Random.Range(_averageTimeBetweenSounds - _maxTimeOffsetOfSounds, _averageTimeBetweenSounds + _maxTimeOffsetOfSounds);
        yield return new WaitForSeconds(timer);
        PlayRandomSound();
        StartCoroutine(RandomSoundCoroutine());
    }


    #endregion

    private void SpawnDamageParticles(Vector2 attackDirection)
    {
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.up, attackDirection);

        _damageParticlesInstance = Instantiate(_damageParticlesPrefab, transform.position, spawnRotation);
    }

}
