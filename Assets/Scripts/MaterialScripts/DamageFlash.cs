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
            SetFlashAmount(0f);
        }
    }

    public void CallDamageFlash()
    {
        foreach (var sprite in _spriteRenderers)
        {
            StartCoroutine(FlashCoroutine());
        }
        
    }

    private IEnumerator FlashCoroutine()
    {
        SetFlashColor();

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        
        while (elapsedTime < _flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, _flashAnimCurve.Evaluate(elapsedTime), (elapsedTime / _flashTime));
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }
    }

    private void SetFlashColor()
    {
        _material.SetColor("_FlashColor", _flashColor);
    }

    private void SetFlashAmount(float amount)
    {
        _material.SetFloat("_FlashAmount", amount);
    }
}
