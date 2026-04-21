using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float regularBulletSpeed = 30f;
    [SerializeField] private float destroyTimer = 3f;
    [SerializeField] private LayerMask layerDestroyingBullet;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetVelocity();

        SetDestroyTime();
    }

    private void SetVelocity()
    {
        rb.linearVelocity = transform.up * regularBulletSpeed;
    }

    private void SetDestroyTime()
    {
        Destroy(gameObject, destroyTimer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & layerDestroyingBullet) != 0)
        {
            Debug.Log("Destroy bullet " + gameObject.name);
            Destroy(gameObject);
        }
    }
}
