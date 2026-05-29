using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _soundFXObject;
    [SerializeField] private AudioSource _musicObject;
    public AudioMixer _audioMixer;
    private List<GameObject> _musicObjectInstances = new List<GameObject>();

    private float _previousMusicVolume;
    private bool _isMusicMuted;

    private float _storedMasterVolume = 0f;
    private float _storedSFXVolume = 0f;
    private float _storedMusicVolume = 0f;

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
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume = 1f, float pitchVariance = 0.05f)
    {
        AudioSource audioSource = Instantiate(_soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;

        if (pitchVariance != 0)
        {
            float randomPitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
            audioSource.pitch = randomPitch;
        }

        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayMusicClip(AudioClip audioClip, Transform spawnTransform, float volume = 1f, bool looping = true)
    {
        AudioSource audioSource = Instantiate(_musicObject, spawnTransform.position, Quaternion.identity);
        _musicObjectInstances.Add(audioSource.gameObject);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.loop = looping;

        audioSource.Play();
    }

    public IEnumerator FadeMusicOut(float duration)
    {
        _audioMixer.GetFloat("musicVolume", out _previousMusicVolume);
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            _audioMixer.SetFloat("musicVolume", Mathf.Lerp(_previousMusicVolume, -80f, elapsedTime / duration));
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    public IEnumerator FadeMusicIn(float duration)
    {
        _audioMixer.GetFloat("musicVolume", out _previousMusicVolume);
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            _audioMixer.SetFloat("musicVolume", Mathf.Lerp(_previousMusicVolume, 0f, elapsedTime / duration));
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    public void MuteMusic()
    {
        if (!_isMusicMuted)
        {
            _audioMixer.GetFloat("musicVolume", out _previousMusicVolume);
            _audioMixer.SetFloat("musicVolume", -80f);
            _isMusicMuted = true;
        }
    }

    public void UnmuteMusic()
    {
        if (!_isMusicMuted)
            return;
        _audioMixer.SetFloat("musicVolume", _previousMusicVolume);
        _isMusicMuted = false;
    }

    public void MuffleMusic()
    {
        foreach(GameObject audioSource in _musicObjectInstances)
        {
            audioSource.GetComponent<AudioLowPassFilter>().cutoffFrequency = 1500f;
        }
    }

    public void UnmuffleMusic()
    {
        foreach(GameObject audioSource in _musicObjectInstances)
        {
            audioSource.GetComponent<AudioLowPassFilter>().cutoffFrequency = 5007.7f;
        }
    }

    public void RemoveMusicSourceFromList(GameObject musicSource)
    {
        _musicObjectInstances.Remove(musicSource);
    }
}
