using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEditor;

public class GameOverUi : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration;

    public UnityEvent OnRestart;
    public UnityEvent OnMainMenu;

    public void RevealUI()
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
        _canvasGroup.DOFade(1f, _fadeDuration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => PauseManager.Instance.PauseGame());
    }

    public void Restart()
    {
        OnRestart.Invoke();
    }

    public void MainMenu()
    {
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
