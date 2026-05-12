using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityIntEvent : UnityEvent<int> { }

public class EnemyBullet : BaseBulletBehavior
{
    public UnityIntEvent OnPlayerHit;

    private void Awake()
    {
        //_healthController = GameObject.FindWithTag("Player").GetComponent<HealthController>();

    }

    protected override void OnBulletHit(Collider2D collision)
    {
        InflictDamageToPlayer();
        DestroyBullet();
    }

    private void InflictDamageToPlayer()
    {
        OnPlayerHit.Invoke(damage);
    }
}
