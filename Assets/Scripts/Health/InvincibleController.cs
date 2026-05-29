using System.Collections;
using UnityEngine;

public class InvincibleController : MonoBehaviour
{
    private HealthController healthController;

    public bool isInvincible;

    private void Awake()
    {
        healthController = GetComponent<HealthController>();
    }

    public void StartInvincibility(float invincibilityDuration)
    {
        if (isInvincible)
            return;
        StartCoroutine(InvincibiltyCoroutine(invincibilityDuration));
    }

    private IEnumerator InvincibiltyCoroutine(float invincibilityDuration) //timer pour l'invincibilité
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration); //Attend pendant duration secondes
        isInvincible = false;
    }
}
