using System.Collections;
using UnityEngine;

public class KnockbackWhenDamaged : MonoBehaviour
{
    private Rigidbody2D _rb;
    private HealthController _healthController;

    [SerializeField] private float knockbackForce = 200f;
    [SerializeField] private float knockbackDuration = 0.2f;
    public bool isKnockedBack { get; private set; } = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _healthController = GetComponent<HealthController>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //if (healthController.isInvincible || isKnockedBack)
            //    return;
            OnEnemyHit(other.collider);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            OnEnemyHit(other);
        }
    }

    private void OnEnemyHit(Collider2D other)
    {
        if (!_healthController.isActiveAndEnabled)
            return;
        Vector2 dir = transform.position - other.transform.position;
        dir.Normalize();
        
        StartCoroutine(KnockbackCoroutine(dir));
    }

    private IEnumerator KnockbackCoroutine(Vector2 dir)
    {
        isKnockedBack = true;
        _rb.linearVelocity = dir * knockbackForce;
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;
    }

}
