using UnityEngine;

public class MoveCrosshair : MonoBehaviour
{
    private Camera _cam;
    private Transform _playerTransform;

    [SerializeField] public float crosshairDistance = 5f;
    [SerializeField] public float crosshairSmoothingTime = 1f;

    private void Start()
    {
        _cam = Camera.main;
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (InputManager.isPlayerLockedOnEnemy)
            return;
        if (InputManager.isGamepad)
        {
            Vector2 dir = InputManager.rightStickDirection;
            if (dir != Vector2.zero)
            {
                Vector2 targetPosition = (Vector2) _cam.transform.position + dir.normalized * crosshairDistance;
                transform.position = Vector2.Lerp(transform.position,  targetPosition, crosshairSmoothingTime);
            }
        }
        else
        {
            Vector2 mousePosition = _cam.ScreenToWorldPoint(InputManager.mousePosition);
            Vector2 directionFromPlayer = (mousePosition - (Vector2)_cam.transform.position).normalized;
            float distanceFromPlayer = Vector2.Distance(mousePosition, _cam.transform.position);
            Vector2 maxPosition = (Vector2) _cam.transform.position + directionFromPlayer * crosshairDistance;
            Vector2 targetPosition = (distanceFromPlayer < crosshairDistance) ? mousePosition : maxPosition;

            transform.position = Vector2.Lerp(transform.position, targetPosition, crosshairSmoothingTime * Time.fixedDeltaTime);
        }
    }
}
