using System.Collections;
using UnityEngine;

public class LifestealBonus : BonusItems
{
    [SerializeField] private float lifestealDuration;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D circleCollider;

    public override void OnPickup()
    {
        StartCoroutine(LifestealCoroutine(lifestealDuration));
    }

    private IEnumerator LifestealCoroutine(float lifestealDuration)
    {
        player.GetComponent<HealthController>().isLifestealBonusEnabled = true;
        spriteRenderer.enabled = false;
        circleCollider.enabled = false;
        yield return new WaitForSeconds(lifestealDuration);
        player.GetComponent<HealthController>().isLifestealBonusEnabled = false;
        Destroy(gameObject);
        
    }

}
