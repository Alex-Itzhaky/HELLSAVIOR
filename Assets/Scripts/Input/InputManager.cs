using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour //j'utilise cet objet pour gérer toutes les inputs
{
    public static Vector2 movement;
    public static Vector2 mousePosition;
    public static bool isPlayerShooting;
    public static bool isPlayerSwappingWeapons;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction mousePositionAction;
    private InputAction shootAction;
    private InputAction swappingAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        moveAction.Enable();
        mousePositionAction = playerInput.actions["CursorPosition"];
        mousePositionAction.Enable();
        shootAction = playerInput.actions["Shoot"];
        shootAction.Enable();
        swappingAction = playerInput.actions["SwapWeapon"];
        swappingAction.Enable();
    }

    private void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
        mousePosition = mousePositionAction.ReadValue<Vector2>();
        isPlayerShooting = shootAction.IsPressed();
        isPlayerSwappingWeapons = swappingAction.WasPressedThisFrame();

        Debug.Log(isPlayerSwappingWeapons);
    }
}
