using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;
    private WeaponHolder weaponHolder;
    private AmmoController ammoController;

    private GameObject bulletInstance;

    private bool canShoot = true;

    private void Awake()
    {
        weaponHolder = GetComponentInChildren<WeaponHolder>();
        ammoController = GetComponentInChildren<AmmoController>();
    }

    private void Update()
    {
        HandleShootingInput();
    }

    private void HandleShootingInput()
    {
        if (InputManager.isPlayerShooting && canShoot)
        {
            ShootGun();
            InputManager.isPlayerShooting = false;
        }

        if (InputManager.isPlayerSwappingWeapons)
        {
            weaponHolder.SwapWeapon();
        }
    }


    private IEnumerator GunFirerateCooldown(float firerate)
    {
        canShoot = false;
        yield return new WaitForSeconds(firerate);
        canShoot = true;
    }

    private IEnumerator GunReload(float reloadTime)
    {
        BaseWeaponData weaponToReload = weaponHolder.currentWeapon;
        yield return new WaitForSeconds(reloadTime);
        ammoController.RefillAmmo(weaponToReload);
    }

    private void ShootGun()
    {
        if (!canShoot)
            return;
        if (!ammoController.HasAmmo(weaponHolder.currentWeapon))
            return;

        InstantiateBullet();
        ammoController.ConsumeAmmo(weaponHolder.currentWeapon);
        if (!ammoController.HasAmmo(weaponHolder.currentWeapon))
            StartCoroutine(GunReload(weaponHolder.currentWeapon.reloadTime));

        StartCoroutine(GunFirerateCooldown(weaponHolder.currentWeapon.firerate));
    }

    private void InstantiateBullet()
    {
        BaseWeaponData weaponData = weaponHolder.currentWeapon;

        GameObject bulletInstance = Instantiate(
            weaponData.bulletPrefab,
            bulletSpawnPoint.position,
            bulletSpawnPoint.rotation
            );
        ApplyBulletSpread(bulletInstance);
        bulletInstance.GetComponent<BaseBulletBehavior>().InitBullet(weaponData);
        
    }

    private void ApplyBulletSpread(GameObject bulletInstance)
    {
        float randomizedSpread = Random.Range(-weaponHolder.currentWeapon.gunSpread, weaponHolder.currentWeapon.gunSpread);
        bulletInstance.transform.Rotate(0, 0, randomizedSpread);
    } 
}
