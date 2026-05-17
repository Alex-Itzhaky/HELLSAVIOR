using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System.Collections;
using UnityEngine.Events;

public class UiPauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _canvasContainer;
    [SerializeField] private float _revealDuration;

    public UnityEvent OnMainMenu;



    private void Awake()
    {
        _canvasContainer.SetActive(false);
    }

    //private void Update()
    //{
    //    if (InputManager.Instance.isOpeningMenu)
    //        if (!PauseManager.Instance.IsPaused)
    //            Pause();
    //    if (InputManager.Instance.isClosingMenuUi)
    //        if (PauseManager.Instance.IsPaused)
    //            Unpause();
    //}
    private void Update()
    {
        if (InputManager.Instance.isOpeningMenu && !PauseManager.Instance.IsPaused)
            StartCoroutine(PauseNextFrame());

        if (InputManager.Instance.isClosingMenuUi && PauseManager.Instance.IsPaused)
            Unpause();
    }

    private IEnumerator PauseNextFrame()
    {
        yield return null;
        Pause();
    }

    public void Pause()
    {
        if (PauseManager.Instance.IsPaused)
            return;
        _canvasContainer.SetActive(true);
        PauseManager.Instance.PauseGame();
    }

    

    public void Unpause()
    {
        if (!PauseManager.Instance.IsPaused)
            return;
        _canvasContainer.SetActive(false);
        PauseManager.Instance.UnpauseGame();
    }

    public void MainMenu()
    {
        PauseManager.Instance.UnpauseGame();
        OnMainMenu.Invoke();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}
