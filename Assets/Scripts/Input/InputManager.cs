using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 movement;
    public static Vector2 mousePosition;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction mouseAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        moveAction.Enable();
        mouseAction = playerInput.actions["CursorPosition"];
        mouseAction.Enable();
    }

    private void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
        mousePosition = mouseAction.ReadValue<Vector2>();
        Debug.Log(mousePosition);
    }
}
