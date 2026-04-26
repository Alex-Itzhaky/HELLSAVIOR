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

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction aimAction;
    private InputAction shootAction;
    private InputAction swappingAction;
    private InputAction lockingAction;
    private InputAction reloadAction;

    private Camera cam;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        moveAction.Enable();
        aimAction = playerInput.actions["Aim"];
        aimAction.Enable();
        shootAction = playerInput.actions["Shoot"];
        shootAction.Enable();
        swappingAction = playerInput.actions["SwapWeapon"];
        swappingAction.Enable();
        lockingAction = playerInput.actions["LockAim"];
        lockingAction.Enable();
        reloadAction = playerInput.actions["Reload"];
        reloadAction.Enable();

        cam = Camera.main;
    }

    private void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
        isPlayerShooting = shootAction.IsPressed();
        isPlayerSwappingWeapons = swappingAction.WasPressedThisFrame();
        isPlayerLockingAim = lockingAction.WasPressedThisFrame();
        isPlayerReloading = reloadAction.WasPressedThisFrame();

        isGamepad = playerInput.currentControlScheme == "Manette";
        if (isGamepad)
        {
            Vector2 rawRightStickDirection = aimAction.ReadValue<Vector2>();
            if (rawRightStickDirection.magnitude > maxRightStickAcceptableMagnitude)
            {
                rightStickDirection = rawRightStickDirection;
            }
        }
        else
            mousePosition = aimAction.ReadValue<Vector2>();

        if (isPlayerLockingAim)
        {
            isPlayerLockedOnEnemy = !isPlayerLockedOnEnemy;
        }
        
    }
}
