using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UiHealthBar : MonoBehaviour
{
    [SerializeField] private HealthController _healthController;
    [SerializeField] private Slider _healthBar;

    [SerializeField] float barSpeed = 5f;

    private void Start()
    {
        _healthBar.minValue = 0f;
        _healthBar.maxValue = 1f;
    }

    public void UpdateHealthBarValue()
    {
        if (_healthController == null || PauseManager.Instance.IsPaused)
            return;
        _healthBar.DOValue(_healthController.RemainingHealthPercentage, barSpeed);     
    }
}
