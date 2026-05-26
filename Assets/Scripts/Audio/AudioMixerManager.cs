using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerManager : MonoBehaviour
{
    public static AudioMixerManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [SerializeField] private AudioMixer _audioMixer;
    public void SetMasterVolume(float volume)
    {
        if (volume < 0.0001f || volume > 1f)
        {
            Debug.LogWarning("Le volume doit rester en 0.0001 et 1");
            return;
        }
        _audioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetSoundFXVolume(float volume)
    {
        if (volume < 0.0001f || volume > 1f)
        {
            Debug.LogWarning("Le volume doit rester en 0.0001 et 1");
            return;
        }
        _audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20f);
    }

    public void SetMusicVolume(float volume)
    {
        if (volume < 0.0001f || volume > 1f)
        {
            Debug.LogWarning("Le volume doit rester en 0.0001 et 1");
            return;
        }
        _audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20f);
    }
}
