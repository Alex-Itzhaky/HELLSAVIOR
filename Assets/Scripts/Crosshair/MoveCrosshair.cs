using DG.Tweening;
using UnityEngine;

public class MoveCrosshair : MonoBehaviour
{
    private Camera _cam;
    private Transform _playerTransform;

    [SerializeField] public float crosshairDistance = 5f;
    [SerializeField] public float crosshairSmoothingTime = 1f;
    private Vector2 _velocityRef;

    private void Start()
    {
        _cam = Camera.main;
        _playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (InputManager.Instance.isPlayerLockedOnEnemy)
            return;
        if (InputManager.Instance.isGamepad)
        {
            Vector2 dir = InputManager.Instance.rightStickDirection;
            if (dir != Vector2.zero)
            {
                Vector2 targetPosition = (Vector2) _playerTransform.position + dir.normalized * crosshairDistance;
                transform.DOMove(targetPosition, crosshairSmoothingTime * Time.deltaTime)
                .SetEase(Ease.InCubic)
                .SetUpdate(UpdateType.Normal);
            }
        }
        else
        {
            Vector2 mousePosition = _cam.ScreenToWorldPoint(InputManager.Instance.mousePosition);
            Vector2 directionFromPlayer = (mousePosition - (Vector2)_playerTransform.position).normalized;

            float distanceFromPlayer = Vector2.Distance(mousePosition, _playerTransform.position);
            Vector2 maxPosition = (Vector2) _playerTransform.position + directionFromPlayer * crosshairDistance;
            Vector2 targetPosition = (distanceFromPlayer < crosshairDistance) ? mousePosition : maxPosition;

            //transform.position = Vector2.SmoothDamp(transform.position, targetPosition, ref _velocityRef, crosshairSmoothingTime * Time.deltaTime);

            transform.position = Vector2.Lerp(transform.position, targetPosition, crosshairSmoothingTime * Time.deltaTime);

            //transform.DOMove(targetPosition, crosshairSmoothingTime * Time.deltaTime)
            //    .SetEase(Ease.InCubic)
            //    .SetUpdate(UpdateType.Normal);
        }
    }
}
