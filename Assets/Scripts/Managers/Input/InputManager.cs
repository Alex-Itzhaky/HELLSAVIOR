using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour //j'utilise cet objet pour gérer toutes les inputs
{
    public Vector2 movement { get; private set; }
    public Vector2 movementFixed { get; private set; }
    public Vector2 mousePosition { get; private set; }
    public Vector2 rightStickDirection { get; private set; }
    public bool isPlayerShooting { get; private set; }
    public bool isPlayerSwappingWeapons { get; private set; }
    public bool isPlayerLockedOnEnemy { get; private set; } = false; //Renvoie le lock en toggle on/off
    public bool isPlayerReloading { get; private set; }
    public bool isOpeningMenu { get; private set; }
    public bool isClosingMenuUi {  get; private set; }

    private bool _isPlayerLockingAim; //Récupère linput

    public bool isGamepad { get; private set; }
    [SerializeField] private float maxRightStickAcceptableMagnitude = 0.1f;

    public static PlayerInput playerInput;

    private InputAction _moveAction;
    private InputAction _aimAction;
    private InputAction _shootAction;
    private InputAction _swappingAction;
    private InputAction _lockingAction;
    private InputAction _reloadAction;
    private InputAction _menuOpenAction;
    private InputAction _uiMenuCloseAction;

    private Camera _cam;

    public static InputManager Instance { get; private set; }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        _moveAction = playerInput.actions["Move"];
        _moveAction.Enable();
        _aimAction = playerInput.actions["Aim"];
        _aimAction.Enable();
        _shootAction = playerInput.actions["Shoot"];
        _shootAction.Enable();
        _swappingAction = playerInput.actions["SwapWeapon"];
        _swappingAction.Enable();
        _lockingAction = playerInput.actions["LockAim"];
        _lockingAction.Enable();
        _reloadAction = playerInput.actions["Reload"];
        _reloadAction.Enable();
        _menuOpenAction = playerInput.actions["MenuOpen"];
        _menuOpenAction.Enable();
        _uiMenuCloseAction = playerInput.actions["MenuClose"];
        _uiMenuCloseAction.Enable();

        _cam = Camera.main;

        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        movement = _moveAction.ReadValue<Vector2>();
        isPlayerShooting = _shootAction.IsPressed();
        isPlayerSwappingWeapons = _swappingAction.WasPressedThisFrame();
        _isPlayerLockingAim = _lockingAction.WasPressedThisFrame();
        isPlayerReloading = _reloadAction.WasPressedThisFrame();
        isOpeningMenu = _menuOpenAction.WasPressedThisFrame();
        isClosingMenuUi = _uiMenuCloseAction.WasPressedThisFrame();


        isGamepad = playerInput.currentControlScheme == "Manette";
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

        if (_isPlayerLockingAim)
        {
            isPlayerLockedOnEnemy = !isPlayerLockedOnEnemy;
        }
        
    }

    private void FixedUpdate()
    {
        movementFixed = _moveAction.ReadValue<Vector2>();
    }

    public void CancelEnemyLocked()
    {
        isPlayerLockedOnEnemy = false;
    }

}
