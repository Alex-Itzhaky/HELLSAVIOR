using System.Collections;
using UnityEditor;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private BaseWeaponData _weaponData;
    [SerializeField] private Transform _bulletSpawnPoint;

    private bool _isPreparingAttack = false;
    private bool _isAttacking = false;
    private bool _canAttack = true;
    [SerializeField] private float _attackDetectionRange;


    private void Start()
    {
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, playerTransform.position);
        _lineRenderer.enabled = false;

        currentHp = enemyData.maxHp;
    }

    private void Update()
    {
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, playerTransform.position);

        if (currentEnemyState != EnemyState.Attacking)
        {
            UpdateTargetDirection();
            RotateTowardsTarget();
        }

        if (currentEnemyState == EnemyState.PreparingAttack)
        {
            _lineRenderer.enabled = true;
        }
        else
        {
            _lineRenderer.enabled = false;
        }

    }

    protected override void HandleEnemyStateMachine()
    {
        switch (currentEnemyState)
        {
            case EnemyState.Chase:
                base.OnChase();
                if (_attackDetectionRange >= Vector2.Distance(playerTransform.position, transform.position) && _canAttack)
                {
                    currentEnemyState = EnemyState.PreparingAttack;
                }
                break;
            case EnemyState.Idle:
                base.OnIdle();
                if (_attackDetectionRange >= Vector2.Distance(playerTransform.position, transform.position) && _canAttack)
                {
                    currentEnemyState = EnemyState.PreparingAttack;
                }
                else if (isPlayerDead)
                {
                    currentEnemyState = EnemyState.Idle;
                }
                else
                {
                    currentEnemyState = EnemyState.Chase;
                }
                break;
            case EnemyState.PreparingAttack:
                if (_isPreparingAttack)
                    return;
                OnPreparingAttack();
                break;
            case EnemyState.Attacking:
                if (_isAttacking)
                    return;
                OnAttacking();
                break;
            case EnemyState.Dead:
                base.OnDead();
                break;
            case EnemyState.GameOver:
                base.OnIdle();
                break;
        }
    }

    protected override void OnPreparingAttack()
    {
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(PrepareAttackCoroutine(enemyData.attackDuration));
    }

    protected override void OnAttacking()
    {
        if (_isAttacking || !_canAttack)
            return;
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(BurstFireCoroutine());
        StartCoroutine(AttackCooldown());
    }



    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_weaponData.reloadTime);
        _canAttack = true;
    }

    private IEnumerator PrepareAttackCoroutine(float duration)
    {
        _isPreparingAttack = true;
        yield return new WaitForSeconds(duration);
        _isPreparingAttack = false;
        currentEnemyState = EnemyState.Attacking;
    }

    private IEnumerator BurstFireCoroutine()
    {
        _isAttacking = true;

        for (int i = 0; i < _weaponData.ammoCount; i++)
        {
            InstantiateBullet();
            yield return new WaitForSeconds(_weaponData.firerate);
        }
        currentEnemyState = EnemyState.Idle;
        _isAttacking = false;
    }



    private void InstantiateBullet()
    {

        GameObject bulletInstance = Instantiate(
            _weaponData.bulletPrefab,
            _bulletSpawnPoint.position,
            _bulletSpawnPoint.rotation
            );
        ApplyBulletSpread(bulletInstance);
        bulletInstance.GetComponent<BaseBulletBehavior>().InitBullet(_weaponData);

    }

    private void ApplyBulletSpread(GameObject bulletInstance)
    {
        float randomizedSpread = Random.Range(-_weaponData.gunSpread, _weaponData.gunSpread);
        bulletInstance.transform.Rotate(0, 0, randomizedSpread);
    }

    

}
