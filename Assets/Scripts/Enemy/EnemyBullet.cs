using UnityEngine;

public class EnemyBullet : BaseBulletBehavior
{
    private HealthController _healthController;

    private void Awake()
    {
        _healthController = GameObject.FindWithTag("Player").GetComponent<HealthController>();
    }

    protected override void OnBulletHit(Collider2D collision)
    {
        InflictDamageToPlayer();
        DestroyBullet();
    }

    private void InflictDamageToPlayer()
    {
        if (_healthController == null)
            return;
        _healthController.TakeDamage(damage);
    }
}
