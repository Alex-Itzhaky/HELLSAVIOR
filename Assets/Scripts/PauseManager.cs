using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        //InputManager.playerInput.SwitchCurrentActionMap("UI");
        Cursor.visible = true;
    }

    public void UnpauseGame()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        Cursor.visible = false;
        //InputManager.playerInput.SwitchCurrentActionMap("Player");
    }
}
