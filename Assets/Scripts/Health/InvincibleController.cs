using DG.Tweening;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class InvincibleController : MonoBehaviour
{
    private HealthController healthController;
    [SerializeField] private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();

    [SerializeField] private float _intervalBetweenFlashes;

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
        foreach (var sprite in _spriteRenderers)
            StartCoroutine(InvincibilityFlash(invincibilityDuration, sprite));
    }

    private IEnumerator InvincibiltyCoroutine(float invincibilityDuration) //timer pour l'invincibilité
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration); //Attend pendant duration secondes
        isInvincible = false;
    }

    private IEnumerator InvincibilityFlash(float invincibilityDuration, SpriteRenderer sprite)
    {
        float baseOpacity = 1f;
        float targetOpacity = 0.5f;

        float timer = 0f;
        while (timer  < invincibilityDuration)
        {
            Tween fadeOutTween = sprite.DOFade(targetOpacity, _intervalBetweenFlashes)
                .SetEase(Ease.InOutCirc);
            yield return fadeOutTween.WaitForCompletion();
            Tween fadeInTween = sprite.DOFade(baseOpacity,_intervalBetweenFlashes)
                .SetEase(Ease.InOutCirc);
            yield return fadeInTween.WaitForCompletion();
            yield return null;
            timer += Time.deltaTime + _intervalBetweenFlashes * 2;
        }
        sprite.DOFade(baseOpacity, _intervalBetweenFlashes).SetEase(Ease.InOutCirc);
    }
}
