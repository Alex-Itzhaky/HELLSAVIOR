using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour //j'utilise cet objet pour gérer toutes les inputs
{
    public static Vector2 movement;
    public static Vector2 mousePosition;
    public static Vector2 rightStickDirection;
    public static bool isPlayerShooting;
    public static bool isPlayerSwappingWeapons;
    public static bool isPlayerLockedOnEnemy = false; //Renvoie le lock en toggle on/off
    public static bool isPlayerReloading;
    
    private bool isPlayerLockingAim; //Récupère linput

    public static bool isGamepad;
    [SerializeField] private float maxRightStickAcceptableMagnitude = 0.1f;

    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _aimAction;
    private InputAction _shootAction;
    private InputAction _swappingAction;
    private InputAction _lockingAction;
    private InputAction _reloadAction;

    private Camera _cam;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        _moveAction.Enable();
        _aimAction = _playerInput.actions["Aim"];
        _aimAction.Enable();
        _shootAction = _playerInput.actions["Shoot"];
        _shootAction.Enable();
        _swappingAction = _playerInput.actions["SwapWeapon"];
        _swappingAction.Enable();
        _lockingAction = _playerInput.actions["LockAim"];
        _lockingAction.Enable();
        _reloadAction = _playerInput.actions["Reload"];
        _reloadAction.Enable();

        _cam = Camera.main;
    }

    private void Update()
    {
        movement = _moveAction.ReadValue<Vector2>();
        isPlayerShooting = _shootAction.IsPressed();
        isPlayerSwappingWeapons = _swappingAction.WasPressedThisFrame();
        isPlayerLockingAim = _lockingAction.WasPressedThisFrame();
        isPlayerReloading = _reloadAction.WasPressedThisFrame();

        isGamepad = _playerInput.currentControlScheme == "Manette";
        if (isGamepad)
        {
            Vector2 rawRightStickDirection = _aimAction.ReadValue<Vector2>();
            if (rawRightStickDirection.magnitude > maxRightStickAcceptableMagnitude)
            {
                rightStickDirection = rawRightStickDirection;
            }
        }
        else
            mousePosition = _aimAction.ReadValue<Vector2>();

        if (isPlayerLockingAim)
        {
            isPlayerLockedOnEnemy = !isPlayerLockedOnEnemy;
        }
        
    }
}
