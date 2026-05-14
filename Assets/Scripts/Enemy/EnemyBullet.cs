using UnityEngine;
using UnityEngine.Events;


public class EnemyBullet : BaseBulletBehavior
{
    private HealthController _healthController;
    private void Awake()
    {
        _healthController = GameObject.FindWithTag("Player").GetComponent<HealthController>();
    }

    private void Start()
    {
        if (_healthController == null)
            DestroyBullet();
    }

    protected override void OnBulletHit(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            InflictDamageToPlayer();
        DestroyBullet();
    }

    private void InflictDamageToPlayer()
    {
        _healthController.TakeDamage(damage);
    }
}
