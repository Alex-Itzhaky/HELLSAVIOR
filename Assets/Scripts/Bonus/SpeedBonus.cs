using System.Collections;
using UnityEngine;

public class SpeedBonus : BonusItems
{
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private float _speedDuration;

    public override void OnPickup()
    {
        StartCoroutine(SpeedCoroutine());
    }

    private IEnumerator SpeedCoroutine()
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.ApplySpeedMultiplier(_speedMultiplier);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(_speedDuration);
        playerMovement.ApplySpeedMultiplier(1f / _speedMultiplier);
        Destroy(gameObject);
    }
}
