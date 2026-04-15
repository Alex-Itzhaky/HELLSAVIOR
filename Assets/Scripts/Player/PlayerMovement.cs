using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 _movement;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _movement.Set(InputManager.movement.x, InputManager.movement.y);

        rb.linearVelocity = _movement * moveSpeed;
    }
}
