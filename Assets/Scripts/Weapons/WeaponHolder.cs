using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private BaseWeaponData[] equippedWeapons = new BaseWeaponData[2];
    private PlayerMovement playerMovement;
    private AmmoController ammoController;

    public BaseWeaponData currentWeapon => equippedWeapons[currentIndex];
    public BaseWeaponData stashedWeapon => equippedWeapons[currentIndex - 1];
    public int currentIndex { get; private set; } = 0;

    

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
        ammoController = GetComponent<AmmoController>();
    }

    public BaseWeaponData GetWeaponAt(int index) => equippedWeapons[index];

    public void SwapWeapon()
    {
        currentIndex = (currentIndex + 1) % equippedWeapons.Length;
        AddWeaponSpeedModifier();
    }

    private void AddWeaponSpeedModifier()
    {
        playerMovement.ResetSpeed();
        playerMovement.ApplyWeaponSpeed(currentWeapon.moveSpeedMultiplier);
    }

    private void EquipWeapon(BaseWeaponData newWeapon, int slotIndex)
    {
        foreach (BaseWeaponData weapon in equippedWeapons)
        {
            if (weapon == newWeapon)
            {
                return;
            }
        }
        equippedWeapons[slotIndex] = newWeapon;
        ammoController.OnWeaponChanged(slotIndex);
    }
    
}
