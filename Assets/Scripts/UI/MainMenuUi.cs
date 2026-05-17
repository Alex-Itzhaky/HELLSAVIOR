using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEditor;

public class MainMenuUi : MonoBehaviour
{
    public UnityEvent OnStart;

    public void StartGame()
    {
        OnStart.Invoke();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}
