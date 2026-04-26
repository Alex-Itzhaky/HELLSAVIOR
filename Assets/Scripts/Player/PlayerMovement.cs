using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 5f;
    private float moveSpeed;

    private Vector2 _movement;

    private Rigidbody2D rb;
    private KnockbackWhenDamaged knockback;

    public void ResetSpeed() => moveSpeed = baseSpeed;
    public void ApplyWeaponSpeed(float moveSpeedMultiplier) => moveSpeed *= moveSpeedMultiplier;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<KnockbackWhenDamaged>();

        ResetSpeed();
    }

    private void FixedUpdate()
    {
        if (knockback.isKnockedBack)
            return;
        _movement.Set(InputManager.movement.x, InputManager.movement.y);

        rb.linearVelocity = _movement * moveSpeed;
    }
}
