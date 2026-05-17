using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AmmoController : MonoBehaviour
{
    private WeaponHolder _weaponHolder;

    private Dictionary<BaseWeaponData, int> _ammoRegistry = new Dictionary<BaseWeaponData, int>();

    public int currentAmmo => _ammoRegistry[_weaponHolder.currentWeapon];
    public bool[] isWeaponReloading { get; private set; } = new bool[2];
    public float[] ElapsedTimeReloading { get; private set; } = {0f ,0f};


    private void Awake()
    {
        _weaponHolder = GetComponent<WeaponHolder>();
    }

    public void Init()
    {
        for (int i = 0; i < 2; i++) //i < 2 pour les deux solts dans WeaponHolder.equippedWeapons
        {
            if (_weaponHolder.GetWeaponAt(i) == null)
                continue;
            RegisterWeapon(_weaponHolder.GetWeaponAt(i));
        }
    }

    private void Update()
    {
        if (isWeaponReloading[_weaponHolder.currentIndex])
            return;
        AutomaticGunReload();
        ManualGunReload();
    }

    private void RegisterWeapon(BaseWeaponData weapon)
    {
        if (!_ammoRegistry.ContainsKey(weapon))
            _ammoRegistry[weapon] = weapon.ammoCount;
    }

    public bool HasAmmo(BaseWeaponData weapon) => _ammoRegistry[weapon] > 0;
    public bool HasMaxAmmo(BaseWeaponData weapon) => _ammoRegistry[weapon] >= weapon.ammoCount;
    public void ConsumeAmmo(BaseWeaponData weapon) => _ammoRegistry[weapon]--;
    public void RefillAmmo(BaseWeaponData weapon) => _ammoRegistry[weapon] = weapon.ammoCount;

    public void OnWeaponChanged(int slotIndex)
    {
        RegisterWeapon(_weaponHolder.GetWeaponAt(slotIndex));
    }
    private IEnumerator GunReloadCoroutine(float reloadTime, int indexToReload)
    {
        isWeaponReloading[indexToReload] = true;
        BaseWeaponData weaponToReload = _weaponHolder.GetWeaponAt(indexToReload);

        while (ElapsedTimeReloading[indexToReload] < reloadTime)
        {
            ElapsedTimeReloading[indexToReload] += Time.deltaTime;
            yield return null;
        }
        RefillAmmo(weaponToReload);
        isWeaponReloading[indexToReload] = false;
        ElapsedTimeReloading[indexToReload] = 0f;
    }

    private void AutomaticGunReload ()
    {
        if (!HasAmmo(_weaponHolder.currentWeapon) && !isWeaponReloading[_weaponHolder.currentIndex])
        {
            StartCoroutine(GunReloadCoroutine(_weaponHolder.currentWeapon.reloadTime, _weaponHolder.currentIndex));
        }
    }

    private void ManualGunReload()
    {
        if (HasAmmo(_weaponHolder.currentWeapon) && !HasMaxAmmo(_weaponHolder.currentWeapon) && InputManager.Instance.isPlayerReloading && !isWeaponReloading[_weaponHolder.currentIndex])
        {
            StartCoroutine(GunReloadCoroutine(_weaponHolder.currentWeapon.reloadTime, _weaponHolder.currentIndex));
        }
    }

    public int GetAmmoAtIndex(int index)
    {
        return _ammoRegistry[_weaponHolder.GetWeaponAt(index)];
    }


    //private void OnGUI()
    //{
    //    int y = 10;
    //    foreach (var entry in _ammoRegistry)
    //    {
    //        GUI.Label(new Rect(10, y, 300, 20), $"{entry.Key.weaponName} : {entry.Value}");
    //        y += 25;
    //    }
    //}
}
