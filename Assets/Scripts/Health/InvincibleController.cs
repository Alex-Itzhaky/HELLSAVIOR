using System.Collections;
using UnityEngine;

public class InvincibleController : MonoBehaviour
{
    private HealthController healthController;

    private void Awake()
    {
        healthController = GetComponent<HealthController>();
    }

    public void StartInvincibility(float invincibilityDuration)
    {
        if (healthController.isInvincible)
            return;
        StartCoroutine(InvincibiltyCoroutine(invincibilityDuration));
    }

    private IEnumerator InvincibiltyCoroutine(float invincibilityDuration) //timer pour l'invincibilité
    {
        healthController.isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration); //Attend pendant duration secondes
        healthController.isInvincible = false;
    }
}
