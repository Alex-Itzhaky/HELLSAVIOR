using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField] public int currentHealth; //Ne pas toucher à cette valeur dans le script
    [SerializeField] private int maximumHealth; //Valeur à changer pour modifier les pv de l'entité

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
    public UnityEvent OnDamagedWhileInvincible;

    private void Awake()
    {
        currentHealth = maximumHealth;
    }

    public void TakeDamage(int damageAmount)
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

    public void TakeTrueDamage(int damageAmount)
    {
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
        if (currentHealth == 0)
        {
            OnDied.Invoke();
        }
        else
        {
            OnDamagedWhileInvincible.Invoke();
        }
    }

    public void AddHealth(int healthReceived)
    {
        currentHealth = Mathf.Min(currentHealth + healthReceived, maximumHealth);
    }
}
