using UnityEngine;

public class RotatePlayer : MonoBehaviour
{
    [SerializeField] private Transform _crosshair;
    [SerializeField] private float smoothingTime;

    private Vector3 _ref;

    private void Update()
    {
        float angle = Mathf.Atan2(_crosshair.position.y - transform.position.y, _crosshair.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
        transform.eulerAngles = new Vector3(0f, 0f, angle);
        //Vector3 rotation = new Vector3(0, 0, angle);
        //transform.eulerAngles = Vector3.SmoothDamp(transform.eulerAngles, rotation, ref _ref, smoothingTime * Time.deltaTime);
    }
}
