using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    private WeaponHolder weaponHolder;

    private Dictionary<BaseWeaponData, int> ammoRegistry = new Dictionary<BaseWeaponData, int>();

    public int currentAmmo => ammoRegistry[weaponHolder.currentWeapon];


    private void Awake()
    {
        weaponHolder = GetComponent<WeaponHolder>();
    }

    private void Start()
    {
        for (int i = 0; i < 2; i++) //i < 2 pour les deux solts dans WeaponHolder.equippedWeapons
        {
            if (weaponHolder.GetWeaponAt(i) == null)
                continue;
            RegisterWeapon(weaponHolder.GetWeaponAt(i));
        }
    }

    private void RegisterWeapon(BaseWeaponData weapon)
    {
        if (!ammoRegistry.ContainsKey(weapon))
            ammoRegistry[weapon] = weapon.ammoCount;
    }

    public bool HasAmmo(BaseWeaponData weapon) => ammoRegistry[weapon] > 0;
    public void ConsumeAmmo(BaseWeaponData weapon) => ammoRegistry[weapon]--;
    public void RefillAmmo(BaseWeaponData weapon) => ammoRegistry[weapon] = weapon.ammoCount;

    public void OnWeaponChanged(int slotIndex)
    {
        RegisterWeapon(weaponHolder.GetWeaponAt(slotIndex));
    }

    



}
