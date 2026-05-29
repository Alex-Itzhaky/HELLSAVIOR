using System.Collections;
using UnityEngine;

public class LifestealBonus : BonusItems
{
    [SerializeField] private float lifestealDuration;

    public override void OnPickup()
    {
        StartCoroutine(LifestealCoroutine(lifestealDuration));
    }

    private IEnumerator LifestealCoroutine(float lifestealDuration)
    {
        player.GetComponent<HealthController>().isLifestealBonusEnabled = true;
        yield return new WaitForSeconds(lifestealDuration);
        player.GetComponent<HealthController>().isLifestealBonusEnabled = false;
        Destroy(gameObject);
    }

}
