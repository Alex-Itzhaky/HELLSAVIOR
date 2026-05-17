using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public BaseWeaponData[] weaponsEquipped { get; private set; } = new BaseWeaponData[2];

    public bool isLoadedFromMainMenu = false;

    public bool isGameOverPlaying { get; private set; } = false;
    public UnityEvent GameOver;

    [SerializeField] private SceneAsset _gameOverUI;
    
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
        isGameOverPlaying = false;
    }

    private void OnSceneUnloaded(Scene scene)
    {

    }

    public void SetPlayerWeapons(BaseWeaponData[] weapons)
    {
        weaponsEquipped = (BaseWeaponData[])weapons.Clone();
    }

    public void TriggerPlayerDeath()
    {
        Debug.Log("TriggerPlayerDeath");
        StartCoroutine(PlayerDeathCoroutine());
    }

    private IEnumerator PlayerDeathCoroutine()
    {
        isGameOverPlaying = true;
        
        yield return new WaitForSecondsRealtime(2);
        PauseManager.Instance.PauseGame();
        yield return SceneManager.LoadSceneAsync(_gameOverUI.name, LoadSceneMode.Additive);

    }
}
