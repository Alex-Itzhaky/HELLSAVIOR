using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;

    private GameObject bulletInstance;

    private const float SHOOT_COOLDOWN = 0.5f;
    private bool canShoot = true;
    
    [SerializeField] private float spreadFactor = 5f;

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
    }

    private IEnumerator GunCooldown()
    {
        float shootCooldown = SHOOT_COOLDOWN;
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    private void ShootGun()
    {
        if (!canShoot)
            return;
        float randomizedSpread = Random.Range(-spreadFactor, spreadFactor);
        bulletInstance = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bulletInstance.transform.Rotate(0, 0, randomizedSpread);
        StartCoroutine(GunCooldown());
    }
}
