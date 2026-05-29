using DG.Tweening;
using System.Collections;
using UnityEngine;

public class InvincibleController : MonoBehaviour
{
    private HealthController healthController;
    [SerializeField] private SpriteRenderer _spriteRenderer;

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
        StartCoroutine(InvincibilityFlash(invincibilityDuration));
    }

    private IEnumerator InvincibiltyCoroutine(float invincibilityDuration) //timer pour l'invincibilité
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration); //Attend pendant duration secondes
        isInvincible = false;
    }

    private IEnumerator InvincibilityFlash(float invincibilityDuration)
    {
        float baseOpacity = 1f;
        float targetOpacity = 0.5f;

        float timer = 0f;
        while (timer  < invincibilityDuration)
        {
            Tween fadeOutTween = _spriteRenderer.DOFade(targetOpacity, _intervalBetweenFlashes)
                .SetEase(Ease.InOutCirc);
            yield return fadeOutTween.WaitForCompletion();
            Tween fadeInTween = _spriteRenderer.DOFade(baseOpacity,_intervalBetweenFlashes)
                .SetEase(Ease.InOutCirc);
            yield return fadeInTween.WaitForCompletion();
            yield return null;
            timer += Time.deltaTime + _intervalBetweenFlashes * 2;
        }
        _spriteRenderer.DOFade(baseOpacity, _intervalBetweenFlashes).SetEase(Ease.InOutCirc);
    }
}
