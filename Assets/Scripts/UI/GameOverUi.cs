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
        PauseManager.Instance.PauseGame();
        Debug.Log("reveal GameOver Ui");
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
        _canvasGroup.DOFade(1f, _fadeDuration)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true)
            .OnComplete(() => { 
                InputManager.Instance.SwitchInputMap(InputManager.ActionMap.UI);
            });
        SoundManager.Instance.MuffleMusic();
    }

    public void Restart()
    {
        OnRestart.Invoke();
        SoundManager.Instance.UnmuffleMusic();
    }

    public void MainMenu()
    {
        OnMainMenu.Invoke();
        SoundManager.Instance.UnmuffleMusic();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    private void OnDestroy()
    {
        InputManager.Instance.SwitchInputMap(InputManager.ActionMap.Player);
    }

}
