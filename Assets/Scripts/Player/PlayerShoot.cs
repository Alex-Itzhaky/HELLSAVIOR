using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform _bulletSpawnPoint;
    private WeaponHolder _weaponHolder;
    private AmmoController _ammoController;

    private GameObject _bulletInstance;

    private bool _canShoot = true;

    private void Awake()
    {
        _weaponHolder = GetComponentInChildren<WeaponHolder>();
        _ammoController = GetComponentInChildren<AmmoController>();
    }

    private void Update()
    {
        HandleSwappingInput();
        if (_ammoController.isWeaponReloading[_weaponHolder.currentIndex])
            return;
        HandleShootingInput();
        //HandleReloadInput();
    }

    private void HandleShootingInput()
    {
        if (InputManager.isPlayerShooting && _canShoot)
        {
            ShootGun();
            InputManager.isPlayerShooting = false;
        }
    }

    private void HandleSwappingInput()
    {
        if (InputManager.isPlayerSwappingWeapons)
        {
            _weaponHolder.SwapWeapon();
        }
    }


    private IEnumerator GunFirerateCooldown(float firerate)
    {
        _canShoot = false;
        yield return new WaitForSeconds(firerate);
        _canShoot = true;
    }

    private void ShootGun()
    {
        if (!_canShoot)
            return;
        if (!_ammoController.HasAmmo(_weaponHolder.currentWeapon))
            return;

        InstantiateBullet();
        _ammoController.ConsumeAmmo(_weaponHolder.currentWeapon);

        StartCoroutine(GunFirerateCooldown(_weaponHolder.currentWeapon.firerate));
    }

    private void InstantiateBullet()
    {
        BaseWeaponData weaponData = _weaponHolder.currentWeapon;

        GameObject bulletInstance = Instantiate(
            weaponData.bulletPrefab,
            _bulletSpawnPoint.position,
            _bulletSpawnPoint.rotation
            );
        ApplyBulletSpread(bulletInstance);
        bulletInstance.GetComponent<BaseBulletBehavior>().InitBullet(weaponData);
        
    }

    private void ApplyBulletSpread(GameObject bulletInstance)
    {
        float randomizedSpread = Random.Range(-_weaponHolder.currentWeapon.gunSpread, _weaponHolder.currentWeapon.gunSpread);
        bulletInstance.transform.Rotate(0, 0, randomizedSpread);
    } 
}
