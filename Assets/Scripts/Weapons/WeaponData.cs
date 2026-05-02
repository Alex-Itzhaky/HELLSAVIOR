using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/WeaponData")]
public class BaseWeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite weaponSprite;
    public GameObject bulletPrefab;
    public int damage;
    public float firerate;
    public float gunSpread;
    public float bulletRange = 50f;
    public int ammoCount;
    public float reloadTime;
    public float moveSpeedMultiplier = 1f;
}
