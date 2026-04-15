using UnityEngine;

public class LookAtCursor : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Vector3 mousePos = (Vector2)cam.ScreenToWorldPoint(InputManager.mousePosition);
        float angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
        float angleDeg = (180 / Mathf.PI) * angleRad - 90;

        transform.rotation = Quaternion.Euler(0f, 0f, angleDeg);

        Debug.DrawLine(transform.position, mousePos, Color.white, Time.deltaTime);
    }
}
