using UnityEngine;

public class LookAtCursor : MonoBehaviour
{

    public GameObject crosshair;
    private Camera cam;

    private Vector3 mousePos = InputManager.mousePosition;

    private void Start()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        crosshair.transform.position = mousePos;
    }

    private void Update()
    {
        mousePos = (Vector2)cam.ScreenToWorldPoint(InputManager.mousePosition);
        float angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
        float angleDeg = (180 / Mathf.PI) * angleRad - 90;

        transform.eulerAngles = new Vector3(0f, 0f, angleDeg);

        Debug.DrawLine(transform.position, mousePos, Color.white, Time.deltaTime);

        
    }
}
