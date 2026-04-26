using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform crosshair;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }
    
    private void LateUpdate()
    {
        /*
        if (InputManager.isPlayerLockedOnEnemy)
        {
            transform.position = Vector2.Lerp(player.position, crosshair.position, cameraWeight);
        }
        else
        {
            Vector2 lookAheadDir;

            if (InputManager.isGamepad)
            {
                lookAheadDir = InputManager.rightStickDirection.normalized;
            }
            else
            {
                Vector2 mousePos = InputManager.mousePosition;
                lookAheadDir = (mousePos - (Vector2)player.transform.position).normalized;
            }

            Vector2 targetPosition = new Vector2(player.position.x + lookAheadDir.x * lookAheadDistance, player.position.y + lookAheadDir.y * lookAheadDistance);
            transform.position = Vector2.Lerp(transform.position, targetPosition, cameraWeight);
        }
        
        
        float interpolationFactor = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;
        Vector2 interpolatedPlayerPos = Vector2.Lerp(previousPlayerPos, (Vector2)player.position, interpolationFactor);
        Vector2 targetPosition = Vector2.Lerp(interpolatedPlayerPos, crosshair.position, cameraWeight);
        transform.position = new Vector3(targetPosition.x, targetPosition.y, -10f);
        */

        transform.position = player.position;
    }
}
