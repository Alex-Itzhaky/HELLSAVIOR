using UnityEngine;

public class PlayerDamagedInvincibility : MonoBehaviour 
{
    private InvincibleController invincibleController;

    [SerializeField] private float invincibilityDuration = 1f; //Cette valeur peut être changée pour allonger la durée d'invincibilité après avoir été damage

    private void Awake()
    {
        invincibleController = GetComponent<InvincibleController>();
    }

    public void StartInvincibility() //Cette fonction est appelée lorsque le joueur reçoit des dégats via le UnityEvent OnDamaged du HealthController
    {
        invincibleController.StartInvincibility(invincibilityDuration);
    }
}
