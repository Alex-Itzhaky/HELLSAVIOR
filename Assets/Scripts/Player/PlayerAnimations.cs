using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;

    //[SerializeField] private Rigidbody2D _rb;
    private bool _isWeapon1 = true;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleBasicAnims();
    }

    private void HandleBasicAnims()
    {
        //if (_rb.linearVelocity.magnitude > .1f)
        //{
        //    _animator.SetBool("IsMoving", true);
        //}
        //else
        //{
        //    _animator.SetBool("IsMoving", false);
        //}
        _animator.SetBool("IsWeapon1", _isWeapon1);
    }

    public void HandleDeathAnim()
    {
        _animator.SetTrigger("IsDead");
    }

    public void SwitchWeaponAnim()
    {
        _isWeapon1 = !_isWeapon1;
    }
}
