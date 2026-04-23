using System.Collections;
using UnityEngine;

public class KnockbackWhenDamaged : MonoBehaviour
{
    private Rigidbody2D rb;
    private HealthController healthController;

    [SerializeField] private float knockbackForce = 200f;
    [SerializeField] private float knockbackDuration = 0.2f;
    public bool isKnockedBack { get; private set; } = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthController = GetComponent<HealthController>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (healthController.isInvincible)
                return;
            Debug.Log("Enemy Hit");
            Vector2 dir = transform.position - other.transform.position;
            dir.Normalize();
            StartCoroutine(KnockbackCoroutine(dir));

            rb.AddForce(dir * knockbackForce);
        }
    }

    private IEnumerator KnockbackCoroutine(Vector2 dir)
    {
        isKnockedBack = true;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir * knockbackForce);
        yield return new WaitForSeconds(knockbackDuration);
        isKnockedBack = false;
    }

}
