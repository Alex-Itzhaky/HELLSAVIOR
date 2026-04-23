using UnityEngine;

public class LucisBullet : BaseBulletBehavior
{
    private float lifeSteal;
    private HealthController healthController;

    [SerializeField] private float healthPercentageFromDamage = 0.25f;

    private void Awake()
    {
        healthController = GameObject.FindWithTag("Player").GetComponent<HealthController>();
    }

    protected override void OnBulletHit(Collider2D collision)
    {
        lifeSteal = Mathf.RoundToInt(damage * healthPercentageFromDamage);
        base.OnBulletHit(collision);
        healthController.AddHealth(lifeSteal);
    }
}
