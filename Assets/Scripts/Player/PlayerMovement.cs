using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _baseSpeed = 5f;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _deceleration = 20f;
    private float _moveSpeed;

    private Vector2 _movement;
    private Vector2 _movementVelocity;

    private Rigidbody2D _rb;
    private KnockbackWhenDamaged _knockback;

    public void ResetSpeed() => _moveSpeed = _baseSpeed;
    public void ApplyWeaponSpeed(float moveSpeedMultiplier) => _moveSpeed *= moveSpeedMultiplier;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _knockback = GetComponent<KnockbackWhenDamaged>();

        ResetSpeed();
    }

    private void FixedUpdate()
    {
        if (_knockback.isKnockedBack)
            return;

        _movement = InputManager.movement;
        if (_movement != Vector2.zero )
        {
            Vector2 targetVelocity = _movement * _moveSpeed;
            _movementVelocity = Vector2.Lerp(_movementVelocity, targetVelocity, _acceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = _movementVelocity;
        }
        else
        {
            Vector2 targetVelocity = Vector2.zero;
            _movementVelocity = Vector2.Lerp(_movementVelocity, targetVelocity, _deceleration * Time.fixedDeltaTime);
            _rb.linearVelocity = _movementVelocity;
        }
    }
}
