using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = 0.25f;
    [SerializeField] private AnimationCurve _flashAnimCurve;

    [SerializeField] private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();
    [SerializeField] private Material _material;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        foreach(var sprite in _spriteRenderers)
        {
            _material = sprite.material;
            SetFlashAmount(0f, sprite);
        }
    }

    public void CallDamageFlash()
    {
        foreach (var sprite in _spriteRenderers)
        {
            StartCoroutine(FlashCoroutine(sprite));
        }
        
    }

    private IEnumerator FlashCoroutine(SpriteRenderer sprite)
    {
        SetFlashColor(sprite);

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        
        while (elapsedTime < _flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, _flashAnimCurve.Evaluate(elapsedTime), (elapsedTime / _flashTime));
            SetFlashAmount(currentFlashAmount, sprite);
            yield return null;
        }
    }

    private void SetFlashColor(SpriteRenderer sprite)
    {
        sprite.material.SetColor("_FlashColor", _flashColor);
    }

    private void SetFlashAmount(float amount, SpriteRenderer sprite)
    {
        sprite.material.SetFloat("_FlashAmount", amount);
    }
}
