using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField] public float currentHealth; //Ne pas toucher à cette valeur dans le script
    [SerializeField] private float maximumHealth; //Valeur à changer pour modifier les pv de l'entité

    public float RemainingHealthPercentage
    {
        get
        {
            return currentHealth / maximumHealth;
        }
    }

    public bool isInvincible;

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

    public void TakeTrueDamage(float damageAmount)
    {
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
        if (currentHealth == 0)
        {
            OnDied.Invoke();
        }
    }

    public void AddHealth(float healthReceived)
    {
        currentHealth = Mathf.Min(currentHealth + healthReceived, maximumHealth);
    }
}
