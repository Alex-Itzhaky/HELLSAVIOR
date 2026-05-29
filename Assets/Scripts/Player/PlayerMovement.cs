using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _baseSpeed = 5f;
    [SerializeField] private float _acceleration = 5f;
    [SerializeField] private float _deceleration = 20f;
    private float _moveSpeed;

    private Vector2 _movementDir;
    private Vector2 _movementVelocity;
    private Vector2 _velocityRef;

    private Rigidbody2D _rb;
    [SerializeField] private KnockbackWhenDamaged _knockback;

    public void ResetSpeed() => _moveSpeed = _baseSpeed;
    public void ApplySpeedMultiplier(float moveSpeedMultiplier) => _moveSpeed *= moveSpeedMultiplier;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        ResetSpeed();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_knockback.isKnockedBack)
            return;
        _movementDir = InputManager.Instance.movementFixed.normalized;
        if (_movementDir != Vector2.zero)
        {
            Vector2 targetVelocity = _movementDir * _moveSpeed;
            //_movementVelocity = Vector2.Lerp(_movementVelocity, targetVelocity, _acceleration * Time.fixedDeltaTime);
            _movementVelocity = Vector2.SmoothDamp(_movementVelocity, targetVelocity, ref _velocityRef, _acceleration * Time.fixedDeltaTime);
        }
        else
        {
            Vector2 targetVelocity = Vector2.zero;
            //_movementVelocity = Vector2.Lerp(_movementVelocity, targetVelocity, _deceleration * Time.fixedDeltaTime);
            _movementVelocity = Vector2.SmoothDamp(_movementVelocity, targetVelocity, ref _velocityRef, _deceleration * Time.fixedDeltaTime);
        }


        _rb.linearVelocity = _movementVelocity;

    }
}
