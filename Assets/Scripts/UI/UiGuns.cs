using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UiGuns : MonoBehaviour
{
    [SerializeField] private WeaponHolder _weaponHolder;
    [SerializeField] private AmmoController _ammoController;
    [SerializeField] private Slider _firstSlider;
    [SerializeField] private Slider _secondSlider;
    [SerializeField] private Image _firstGunImage;
    [SerializeField] private Image _secondGunImage;
    [SerializeField] private TextMeshProUGUI _firstAmmoField;
    [SerializeField] private TextMeshProUGUI _secondAmmoField;
    [SerializeField] private Image _firstBackground;
    [SerializeField] private Image _secondBackground;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _unselectedColor;
    [SerializeField] private float _tweeningColorDuration;
    [SerializeField] private float _tweeningSliderDuration;

    private bool isFirstGunSelected = true;


    private void Start()
    {
        _firstSlider.maxValue = _weaponHolder.GetWeaponAt(0).reloadTime;
        _secondSlider.maxValue = _weaponHolder.GetWeaponAt(1).reloadTime;
        _firstSlider.value = 0f;
        _secondSlider.value = 0f;

        _firstGunImage.sprite = _weaponHolder.GetWeaponAt(0).uiWeaponImage;
        _secondGunImage.sprite = _weaponHolder.GetWeaponAt(1).uiWeaponImage;
        _firstAmmoField.text = $"{_weaponHolder.GetWeaponAt(0).ammoCount}/{_weaponHolder.GetWeaponAt(0).ammoCount}";
        _secondAmmoField.text = $"{_weaponHolder.GetWeaponAt(1).ammoCount}/{_weaponHolder.GetWeaponAt(1).ammoCount}";
        _firstBackground.color = _selectedColor;
        _secondBackground.color = _unselectedColor;
    }

    private void Update()
    {
        _firstAmmoField.text = $"{_ammoController.GetAmmoAtIndex(0)}/{_weaponHolder.GetWeaponAt(0).ammoCount}";
        _secondAmmoField.text = $"{_ammoController.GetAmmoAtIndex(1)}/{_weaponHolder.GetWeaponAt(1).ammoCount}";

        _firstSlider.value = _ammoController.ElapsedTimeReloading[0];
        _secondSlider.value = _ammoController.ElapsedTimeReloading[1];

        if (_firstSlider.value >= _weaponHolder.GetWeaponAt(0).reloadTime)
        {
            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(_firstSlider.DOValue(0f, _tweeningSliderDuration));
            _sequence.SetEase(Ease.InCubic);
            _sequence.Play();
        }
            
        if (_secondSlider.value >= _weaponHolder.GetWeaponAt(1).reloadTime)
        {
            Sequence _sequence = DOTween.Sequence();
            _sequence.Append(_secondSlider.DOValue(0f, _tweeningSliderDuration));
            _sequence.SetEase(Ease.InCubic);
            _sequence.Play();
        }
    }

    public void SwapWeaponPanel()
    {
        
        if (isFirstGunSelected)
        {
            _firstBackground.DOColor(_unselectedColor, _tweeningColorDuration);
            _secondBackground.DOColor(_selectedColor, _tweeningColorDuration);
            isFirstGunSelected = false;
        }
        else
        {
            _firstBackground.DOColor(_selectedColor, _tweeningColorDuration);
            _secondBackground.DOColor(_unselectedColor, _tweeningColorDuration);
            isFirstGunSelected = true;
        }
    }
}
