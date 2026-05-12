using UnityEngine;
using UnityEngine.UI;

public class UiHealthBar : MonoBehaviour
{
    [SerializeField] private HealthController _healthController;
    [SerializeField] private Slider _healthBar;

    private void Start()
    {
        _healthBar.minValue = 0f;
        _healthBar.maxValue = 1f;
    }

    private void Update()
    {
        if (_healthController == null)
            return;
        _healthBar.value = _healthController.RemainingHealthPercentage;        
    }
}
