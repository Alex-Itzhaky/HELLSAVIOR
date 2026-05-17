using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class GunEquipUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _weaponNameText;
    [SerializeField] private TextMeshProUGUI _weaponDescriptionText;

    [SerializeField] private Image _firstWeaponImage;
    [SerializeField] private Image _secondWeaponImage;

    [SerializeField] private Button _firstWeaponButton;
    [SerializeField] private Button _secondWeaponButton;

    private BaseWeaponData[] _chosenWeapons = new BaseWeaponData[2];
    [SerializeField] private BaseWeaponData[] _availableWeapons = new BaseWeaponData[5];

    private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeDuration;

    public UnityEvent Started;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void RevealGunUi()
    {
        _canvasGroup.DOFade(1f, _fadeDuration).SetEase(Ease.InOutCirc);
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void ShowDescription(BaseWeaponData weapon)
    {
        _weaponNameText.text = weapon.weaponName;
        _weaponDescriptionText.text = weapon.weaponDescription;
    }

    public void EquipNewWeapon(BaseWeaponData weapon)
    {
        bool alreadyChosen = System.Array.IndexOf(_chosenWeapons, weapon) != -1;
        bool isAvailable = System.Array.IndexOf(_availableWeapons, weapon) != -1;
        int emptySlot = System.Array.IndexOf(_chosenWeapons, null);

        if (!alreadyChosen && isAvailable && emptySlot != -1)
        {
            _chosenWeapons[emptySlot] = weapon;
            _availableWeapons[System.Array.IndexOf(_availableWeapons, weapon)] = null;

            if (emptySlot == 0)
            {
                _firstWeaponImage.enabled = true;
                _firstWeaponImage.sprite = weapon.uiWeaponImage;
                _firstWeaponButton.interactable = true;
            }
            else
            {
                _secondWeaponImage.enabled = true;
                _secondWeaponImage.sprite = weapon.uiWeaponImage;
                _secondWeaponButton.interactable = true;
            }
        }
    }

    public void UnequipWeapon(int slotIndex)
    {
        BaseWeaponData weapon = _chosenWeapons[slotIndex];
        if (weapon == null) return;

        _chosenWeapons[slotIndex] = null;
        _availableWeapons[System.Array.IndexOf(_availableWeapons, null)] = weapon;

        if (slotIndex == 0)
        {
            _firstWeaponImage.enabled = false;
            _firstWeaponImage.sprite = null;
            _firstWeaponButton.interactable = false;
        }
        else
        {
            _secondWeaponImage.enabled = false;
            _secondWeaponImage.sprite = null;
            _secondWeaponButton.interactable = false;
        }
    }

    public void OnStart()
    {
        if (_chosenWeapons[0] == null || _chosenWeapons[1] == null) 
            return;
        GameManager.Instance.SetPlayerWeapons(_chosenWeapons);
        GameManager.Instance.isLoadedFromMainMenu = true;
        Started.Invoke();
    }
}
