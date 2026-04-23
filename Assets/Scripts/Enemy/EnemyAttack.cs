using UnityEngine;

public class EnemyAttack : MonoBehaviour //Ce script est juste temporaire pour tester les mécaniques
                                         //Ce genre de script n'est utilse que si l'ennemi n'a qu'une seule attaque
{
    [SerializeField] private float attackDamage; //Valeur à changer dans l'inspecteur

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthController healthController = collision.gameObject.GetComponent<HealthController>();

            healthController.TakeDamage(attackDamage);
        }
    }
}
