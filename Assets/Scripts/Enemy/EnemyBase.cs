using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamgeable
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("currentHealth = " + currentHealth);

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
