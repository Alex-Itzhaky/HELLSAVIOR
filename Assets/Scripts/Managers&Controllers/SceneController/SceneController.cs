using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private float _sceneFadeDuration;
    [SerializeField] private SceneFade _sceneFade;

    public void LoadScene(SceneAsset scene)
    {
        StartCoroutine(LoadSceneCoroutine(scene.name));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return _sceneFade.FadeOutCoroutine(_sceneFadeDuration);
        yield return SceneManager.LoadSceneAsync(sceneName);
        PauseManager.Instance.UnpauseGame();
    }
}
