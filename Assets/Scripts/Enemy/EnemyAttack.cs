using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackDamage;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthController healthController = collision.gameObject.GetComponent<HealthController>();

            healthController.TakeDamage(attackDamage);
        }
    }
}
