using UnityEngine;

public class HealthBonus : BonusItems
{

    [SerializeField] private int healAmount;
    public override void OnPickup()
    {
        HealthController healthController = player.GetComponent<HealthController>();
        if (healthController.RemainingHealthPercentage < 1)
        {
            healthController.AddHealth(healAmount);
            Destroy(gameObject);
        }
    }
}
