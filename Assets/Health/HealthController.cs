using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maximumHealth;

    public float RemainingHealthPercentage
    {
        get
        {
            return currentHealth / maximumHealth;
        }
    }

    public bool isInvincible { get; set; }

    public UnityEvent OnDied;
    public UnityEvent OnDamaged;

    public void TakeDamage(float damageAmount)
    {
        if (isInvincible)
            return;

        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
        if (currentHealth == 0)
        {
            OnDied.Invoke();
        }
        else
        {
            OnDamaged.Invoke();
        }
    }

    public void AddHealth(float healthReceived)
    {
        currentHealth = Mathf.Min(currentHealth + healthReceived, maximumHealth);
    }
}
