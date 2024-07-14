using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SOComboWeapon : ScriptableObject
{
    [Header("Weapon Components")]
    public string displayName;
    public Sprite weaponSprite;
    public Sprite weaponIcon;
    public Weapon weaponType;
    [TextArea]
    public string descriptionText;

    [Header("Weapon Stats")]
    public float drainRate;

    public abstract void StartComboWeapon(PlayerWeapon playerWeapon);

    public abstract void EndComboWeapon(PlayerWeapon playerWeapon);

    public virtual void OnUpdate(PlayerWeapon playerWeapon)
    {

    }

    public virtual void OnFire(PlayerWeapon playerWeapon, SOWeapon firedWeapon)
    {

    }

    public virtual void OnProjectileSpawn(Projectile projectile)
    {

    }
}
