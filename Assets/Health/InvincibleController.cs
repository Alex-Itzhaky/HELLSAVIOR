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
        StartCoroutine(InvincibiltyCoroutine(invincibilityDuration));
    }

    public IEnumerator InvincibiltyCoroutine(float invincibilityDuration)
    {
        healthController.isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        healthController.isInvincible = false;
    }
}
