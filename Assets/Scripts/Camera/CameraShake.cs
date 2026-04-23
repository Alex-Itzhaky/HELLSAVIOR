using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    public static CameraShake cameraInstance;
    private void Awake() => cameraInstance = this;

    public void StartCameraShake(float duration = 0.2f, float magnitude = 0.2f) //Garder la magnitude entre 0 et 0.5 pour des effets lisibles
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude) 
    {
        Vector3 originalPos = transform.localPosition;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
            float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;

            transform.localPosition = new Vector3(xOffset, yOffset, transform.localPosition.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    } 
}
