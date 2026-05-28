using UnityEngine;
using UnityEngine.Events;

public class WeaponHolder : MonoBehaviour
{
    private WeaponData[] _equippedWeapons = new WeaponData[2];
    private PlayerMovement _playerMovement;
    private AmmoController _ammoController;
    [SerializeField] private UiGuns _gunsUi;

    public WeaponData currentWeapon => _equippedWeapons[currentIndex];
    public WeaponData stashedWeapon => _equippedWeapons[currentIndex - 1];
    public int currentIndex { get; private set; } = 0;

    public UnityEvent OnWeaponSwapped;

    public UnityEvent WeaponHolderInitError;

    

    private void Awake()
    {
        _playerMovement = GetComponentInParent<PlayerMovement>();
        _ammoController = GetComponent<AmmoController>();
    }

    private void Start()
    {
        if (GameManager.Instance.weaponsEquipped[0] == null && GameManager.Instance.weaponsEquipped[1] == null)
        { 
            Debug.LogError("weaponsEquipped est null. Lancez le jeu depuis la scene MainMenu");
            WeaponHolderInitError.Invoke();
            return; 
        }

        EquipWeapon(GameManager.Instance.weaponsEquipped[0], 0);
        EquipWeapon(GameManager.Instance.weaponsEquipped[1], 1);

        _ammoController.Init();
        _gunsUi.Init();
        AddWeaponSpeedModifier();
    }

    public WeaponData GetWeaponAt(int index) => _equippedWeapons[index];

    public void SwapWeapon()
    {
        currentIndex = (currentIndex + 1) % _equippedWeapons.Length;
        AddWeaponSpeedModifier();
        OnWeaponSwapped.Invoke();
    }

    private void AddWeaponSpeedModifier()
    {
        if (currentWeapon == null)
            return;
        _playerMovement.ResetSpeed();
        _playerMovement.ApplyWeaponSpeed(currentWeapon.moveSpeedMultiplier);
    }

    private void EquipWeapon(WeaponData newWeapon, int slotIndex)
    {
        foreach (WeaponData weapon in _equippedWeapons)
        {
            if (weapon == newWeapon)
            {
                return;
            }
        }
        _equippedWeapons[slotIndex] = newWeapon;
        _ammoController.OnWeaponChanged(slotIndex);
        AddWeaponSpeedModifier();
    }

    //Debug EquipWeapon
    [SerializeField] private WeaponData testWeapon;
    [SerializeField] private int testSlotIndex;

    [ContextMenu("Test Equip Weapon")]

    private void TestEquipWeapon()
    {
        EquipWeapon(testWeapon, testSlotIndex);
    }

}
