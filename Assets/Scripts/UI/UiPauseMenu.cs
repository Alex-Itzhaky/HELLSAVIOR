using UnityEngine;
using DG.Tweening;
using UnityEditor;
using System.Collections;
using UnityEngine.Events;

public class UiPauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _canvasContainer;
    [SerializeField] private float _revealDuration;
    [SerializeField] private RectTransform _pauseMenu;
    [SerializeField] private RectTransform _optionsMenu;
    [SerializeField] private float _menuTransitionsDuration;
    [SerializeField] private float _xOffset;


    private void Awake()
    {
        _canvasContainer.SetActive(false);
    }

    private void Start()
    {
        _optionsMenu.position = new Vector3(_xOffset, _optionsMenu.position.y, _optionsMenu.position.z);
        _optionsMenu.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOverPlaying)
        {
            Unpause();
            return;
        }
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
        InputManager.Instance.SwitchInputMap(InputManager.ActionMap.UI);
        Debug.Log($"Current action Map : {InputManager.Instance.playerInput.currentActionMap}");
    }

    

    public void Unpause()
    {
        if (!PauseManager.Instance.IsPaused)
            return;
        _canvasContainer.SetActive(false);
        PauseManager.Instance.UnpauseGame();
        InputManager.Instance.SwitchInputMap(InputManager.ActionMap.Player);
        Debug.Log($"Current action Map : {InputManager.Instance.playerInput.currentActionMap}");
    }

    public void MainMenu()
    {
        Unpause();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    public void OpenOptionsMenu()
    {
        _pauseMenu.DOLocalMoveX(-_xOffset, _menuTransitionsDuration)
            .SetEase(Ease.InCubic)
            .SetUpdate(true)
            .OnComplete(() => {
            _pauseMenu.gameObject.SetActive(false);
            _optionsMenu.gameObject.SetActive(true);
            _optionsMenu.DOLocalMoveX(0f, _menuTransitionsDuration)
            .SetEase(Ease.OutCubic)
            .SetUpdate(true);
        });

        
    }

    public void CloseOptionsMenu()
    {
        _optionsMenu.DOLocalMoveX(_xOffset, _menuTransitionsDuration)
            .SetEase(Ease.InCubic)
            .SetUpdate(true)
            .OnComplete(() => {
                _optionsMenu.gameObject.SetActive(false);
                _pauseMenu.gameObject.SetActive(true);
                _pauseMenu.DOLocalMoveX(0f, _menuTransitionsDuration)
                .SetEase(Ease.OutCubic)
                .SetUpdate(true);
            });
    }
}
