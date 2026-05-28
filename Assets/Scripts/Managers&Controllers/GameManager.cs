using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public WeaponData[] weaponsEquipped { get; private set; } = new WeaponData[2];

    public bool isLoadedFromMainMenu = false;

    public bool isGameOverPlaying { get; private set; } = false;
    public UnityEvent GameOver;

    [SerializeField] private GameOverUi _gameOverUI;
    [SerializeField] private AudioClip _gameMusic;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        PauseManager.Instance.UnpauseGame();
        if (scene.name == "ScenePrototype" && !isLoadedFromMainMenu)
        {
            Debug.LogWarning("Le jeu n'a pas été lancé depuis le menu principal. Redirection forcée vers la scène MainMenu...");
            SceneManager.LoadScene("MainMenu");
        }
        if (scene.name == "MainMenu" && !isLoadedFromMainMenu)
            isLoadedFromMainMenu = true;
        if (scene.name == "ScenePrototype" && isLoadedFromMainMenu)
            SoundManager.Instance.PlayMusicClip(_gameMusic, transform);


        isGameOverPlaying = false;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        InputManager.Instance.SwitchInputMap(InputManager.ActionMap.Player);
    }

    public void SetPlayerWeapons(WeaponData[] weapons)
    {
        weaponsEquipped = (WeaponData[])weapons.Clone();
    }

    public void TriggerPlayerDeath()
    {
        isGameOverPlaying = true;
        GameOver.Invoke();
    }
}
