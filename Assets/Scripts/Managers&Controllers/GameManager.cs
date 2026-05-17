using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public BaseWeaponData[] weaponsEquipped { get; private set; } = new BaseWeaponData[2];
    
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
    }

    private void OnSceneUnloaded(Scene scene)
    {

    }

    public void SetPlayerWeapons(BaseWeaponData[] weapons)
    {
        weaponsEquipped = (BaseWeaponData[])weapons.Clone();
    }
}
