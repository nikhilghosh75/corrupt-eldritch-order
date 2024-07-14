using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Project Classic/Weapon")]
public class SOWeapon : ScriptableObject
{
    [Header("Weapon Components")]
    public Sprite weaponSprite;
    public Sprite weaponIcon;
    public GameObject bulletPrefab;
    public WeaponType type;
    public Weapon weapon;
    public bool unlockedByDefault = false;

    [Header("Weapon Stats")]
    public int damage = 1;
    public float fireCooldown = 0.3f;
    public float bulletSpeed = 5f;
    public float timeUntilDestroy = 3f;
    // set to a degree value, e.g 30 will allow bullets to spread randomly within a 30 degree range.
    public int bulletSpread = 0;
    // for now this number directly reflects the number of things the bullet can go through
    // so a value of 1 means it can pierce through 1 enemy, 2 means 2 enemies, etc
    public int pierce = 0;
    public int manaRecovered;
    public int manaConsumed;
    public float shotgunSpread = 0;
    public float bulletsFired = 1;
}