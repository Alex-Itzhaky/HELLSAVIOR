using UnityEngine;

public class InvincibilityBonus : BonusItems
{
    [SerializeField] private float invincibilityDuration;
    public override void OnPickup()
    {
        InvincibleController invincibleController = player.GetComponent<InvincibleController>();
        if (!invincibleController.isInvincible)
        {
            invincibleController.StartInvincibility(invincibilityDuration);
            Destroy(gameObject);
        }
    }
}
