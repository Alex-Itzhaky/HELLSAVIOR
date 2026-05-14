using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiWaveProgressBar : MonoBehaviour
{
    [SerializeField] private WaveManager _waveManager;
    [SerializeField] private Slider _waveProgressBar;
    [SerializeField] private TextMeshPro _textField;


    private void Start()
    {
        _waveProgressBar.minValue = 0f;
        _waveProgressBar.maxValue = 1f;
        _waveProgressBar.value = 0f;
    }

    private void Update()
    {
        if (_waveManager == null)
            return;

        if (_waveManager.CurrentWaveState == WaveManager.WaveState.Start)
            _waveProgressBar.value = 0f;
        else if (_waveManager.CurrentWaveState == WaveManager.WaveState.Playing)
            _waveProgressBar.value = _waveManager.ElapsedTimePercentage;
        else if (_waveManager.CurrentWaveState == WaveManager.WaveState.Transitioning)
            _waveProgressBar.value = 1f;
        else
            _waveProgressBar.value = 0f;
    }
}
