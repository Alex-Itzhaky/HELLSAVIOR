using System.Collections;
using UnityEditor;
using UnityEngine;

public class VitasBullet : BaseBulletBehavior
{
    private HealthController healthController;

    [SerializeField] private float blastRadius = 1f;
    [SerializeField] private float blastDuration = 0.5f;
    [SerializeField] private float healthDrain = 5f;

    private bool isExploding = false;
    private Vector2 blastOrigin;
    

    private void Awake()
    {
        healthController = GameObject.FindWithTag("Player").GetComponent<HealthController>();
        if (healthController == null)
            return;
        healthController.TakeTrueDamage(healthDrain);
    }

    protected override void OnBulletHit(Collider2D collision)
    {
        StartCoroutine(ExplodeBullet(collision));
        CameraShake.cameraInstance.StartCameraShake(blastDuration, 0.3f);
        DestroyBullet();
    }

    private IEnumerator ExplodeBullet(Collider2D collision)
    {
        isExploding = true;

        blastOrigin = collision.transform.position;
        Collider2D[] collisionHits = Physics2D.OverlapCircleAll(blastOrigin, blastRadius);
        foreach (Collider2D collisionHit in collisionHits)
        {
            IDamgeable iDamageable = collisionHit.GetComponent<IDamgeable>();
            if (iDamageable != null)
            {
                iDamageable.Damage(damage);
            }
        }
        yield return new WaitForSeconds(blastDuration);
        isExploding = false;
    }

    private void OnDrawGizmos()
    {
        if (!isExploding)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(blastOrigin, blastRadius);
    }
}
