using UnityEngine;

public class PlayerDamagedInvincibility : MonoBehaviour
{
    private InvincibleController invincibleController;

    [SerializeField] private float invincibilityDuration = 1f;

    private void Awake()
    {
        invincibleController = GetComponent<InvincibleController>();
    }

    public void StartInvincibility()
    {
        invincibleController.StartInvincibility(invincibilityDuration);
    }
}
