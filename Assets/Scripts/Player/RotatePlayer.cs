using UnityEngine;

public class RotatePlayer : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private Transform crosshair;

    private void Start()
    {
        cam = Camera.main;
    }


    private void Update()
    {
        /*
        if (InputManager.isGamepad)
        {
            Vector2 dir = InputManager.rightStickDirection;
            if (dir != Vector2.zero)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
                transform.eulerAngles = new Vector3(0, 0, angle);
            }
        }
        else
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(InputManager.mousePosition);
            float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg - 90;

            transform.eulerAngles = new Vector3(0f, 0f, angle);
        }
        */
        float angle = Mathf.Atan2(crosshair.position.y - transform.position.y, crosshair.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
        transform.eulerAngles = new Vector3(0f, 0f, angle);
        
    }
}
