using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] PlayerMovement _movement;
    [SerializeField] RotatePlayer _rotatePlayer;
    [SerializeField] PlayerShoot _shoot;
    [SerializeField] Rigidbody2D _rb;


    public void OnDead()
    {
        _movement.enabled = false;
        _rotatePlayer.enabled = false;
        _rb.simulated = false;
        _shoot.enabled = false;

    }
}
