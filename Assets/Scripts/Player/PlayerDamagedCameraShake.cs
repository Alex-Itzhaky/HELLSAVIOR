using UnityEngine;

public class PlayerDamagedCameraShake : MonoBehaviour
{
    public void StartCameraShake()
    {
        CameraShake.cameraInstance.StartCameraShake(0.2f, 0.2f);
    }
}
