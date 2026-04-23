using UnityEngine;

public class FPSCap : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        //Debug.Log("FPS : " + 1.0f / Time.deltaTime);
    }
}
