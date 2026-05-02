using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private BaseWeaponData[] _equippedWeapons = new BaseWeaponData[2];
    private PlayerMovement _playerMovement;
    private AmmoController _ammoController;

    public BaseWeaponData currentWeapon => _equippedWeapons[currentIndex];
    public BaseWeaponData stashedWeapon => _equippedWeapons[currentIndex - 1];
    public int currentIndex { get; private set; } = 0;

    

    private void Awake()
    {
        _playerMovement = GetComponentInParent<PlayerMovement>();
        _ammoController = GetComponent<AmmoController>();
    }

    private void Start()
    {
        AddWeaponSpeedModifier();
    }

    public BaseWeaponData GetWeaponAt(int index) => _equippedWeapons[index];

    public void SwapWeapon()
    {
        currentIndex = (currentIndex + 1) % _equippedWeapons.Length;
        AddWeaponSpeedModifier();
    }

    private void AddWeaponSpeedModifier()
    {
        _playerMovement.ResetSpeed();
        if (currentWeapon != null)
            return;
        _playerMovement.ApplyWeaponSpeed(currentWeapon.moveSpeedMultiplier);
    }

    private void EquipWeapon(BaseWeaponData newWeapon, int slotIndex)
    {
        foreach (BaseWeaponData weapon in _equippedWeapons)
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
    [SerializeField] private BaseWeaponData testWeapon;
    [SerializeField] private int testSlotIndex;

    [ContextMenu("Test Equip Weapon")]

    private void TestEquipWeapon()
    {
        EquipWeapon(testWeapon, testSlotIndex);
    }

}
