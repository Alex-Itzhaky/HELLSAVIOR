using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    private WeaponHolder weaponHolder;

    private Dictionary<BaseWeaponData, int> ammoRegistry = new Dictionary<BaseWeaponData, int>();

    public int currentAmmo => ammoRegistry[weaponHolder.currentWeapon];
    public bool[] isWeaponReloading { get; private set; } = new bool[2];


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

    private void Update()
    {
        if (isWeaponReloading[weaponHolder.currentIndex])
            return;
        AutomaticGunReload();
        ManualGunReload();
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
    private IEnumerator GunReloadCoroutine(float reloadTime, int indexToReload)
    {
        isWeaponReloading[indexToReload] = true;
        BaseWeaponData weaponToReload = weaponHolder.GetWeaponAt(indexToReload);
        yield return new WaitForSeconds(reloadTime);
        RefillAmmo(weaponToReload);
        isWeaponReloading[indexToReload] = false;
    }

    private void AutomaticGunReload ()
    {
        if (!HasAmmo(weaponHolder.currentWeapon))
        {
            StartCoroutine(GunReloadCoroutine(weaponHolder.currentWeapon.reloadTime, weaponHolder.currentIndex));
        }
    }

    private void ManualGunReload()
    {
        if (HasAmmo(weaponHolder.currentWeapon) && InputManager.isPlayerReloading)
        {
            StartCoroutine(GunReloadCoroutine(weaponHolder.currentWeapon.reloadTime, weaponHolder.currentIndex));
        }
    }


    private void OnGUI()
    {
        int y = 10;
        foreach (var entry in ammoRegistry)
        {
            GUI.Label(new Rect(10, y, 300, 20), $"{entry.Key.weaponName} : {entry.Value}");
            y += 25;
        }
    }
}
