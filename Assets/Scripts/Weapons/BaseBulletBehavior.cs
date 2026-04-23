using UnityEngine;

public class BaseBulletBehavior : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private LayerMask layerDestroyingBullet; //Layers détectées par la collision des balles
    private Vector2 bulletOriginPosition;

    protected float damage;
    private float bulletRange;

    [SerializeField] private float bulletSpeed = 100f;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetVelocity();
        SetOriginPosition();
    }

    private void Update()
    {
        CheckDistanceTravelled();
    }

    public void InitBullet(BaseWeaponData weapon)
    {
        damage = weapon.damage;
        bulletRange = weapon.bulletRange;
    }

    private void SetVelocity()
    {
        rb.linearVelocity = transform.up * bulletSpeed;
    }

    private void SetOriginPosition()
    {
        bulletOriginPosition = transform.position;
    }

    private void CheckDistanceTravelled()
    {
        float distance = Vector2.Distance(bulletOriginPosition, transform.position);
        if (distance >= bulletRange)
        {
            DestroyBullet();
        }
    }


    protected virtual void OnBulletHit(Collider2D collision)
    {
        InflictDamageOnCollision(collision);
        DestroyBullet();
    }

    private void InflictDamageOnCollision(Collider2D collision)
    {
        IDamgeable iDamageable = collision.gameObject.GetComponent<IDamgeable>();
        if (iDamageable != null)
        {
            iDamageable.Damage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & layerDestroyingBullet) != 0)
        {
            OnBulletHit(collision);
        }
    }

    protected void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
