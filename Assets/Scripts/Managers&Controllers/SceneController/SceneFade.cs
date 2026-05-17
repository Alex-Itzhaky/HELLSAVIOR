using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    private Image _sceneFadeImage;

    private void Awake()
    {
        _sceneFadeImage = GetComponent<Image>();
    }

    public IEnumerator FadeInCoroutine(float duration)
    {
        Color startColor = _sceneFadeImage.color;
        startColor.a = 1;
        Color endColor = _sceneFadeImage.color;
        endColor.a = 0;

        yield return FadeCoroutine(startColor, endColor, duration);

        gameObject.SetActive(false);
    }

    public IEnumerator FadeOutCoroutine(float duration)
    {
        Color startColor = _sceneFadeImage.color;
        startColor.a = 0;
        Color endColor = _sceneFadeImage.color;
        endColor.a = 1;

        gameObject.SetActive(true);
        yield return FadeCoroutine(startColor, endColor, duration);
    }

    private IEnumerator FadeCoroutine(Color startColor, Color endColor, float duration)
    {
        float elapsedTime = 0f;
        float elapsedPercentage = 0f;
        

        while (elapsedPercentage < 1)
        {
            elapsedPercentage = elapsedTime / duration;
            _sceneFadeImage.color = Color.Lerp(startColor, endColor, elapsedPercentage);

            yield return null;
            elapsedTime += Time.unscaledDeltaTime;
        }
    }
}
