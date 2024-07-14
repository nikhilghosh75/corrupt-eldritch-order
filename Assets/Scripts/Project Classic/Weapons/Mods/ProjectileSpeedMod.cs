using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Speed Mod", menuName = "Project Classic/Mods/Projectile Speed Mod")]
public class ProjectileSpeedMod : WeaponMod
{
    public float speedMultiplier;

    public override void OnModEquipped()
    {
        base.OnModEquipped();

        PlayerController.Instance.playerWeapon.bulletSpeedMultiplier *= speedMultiplier;
    }

    public override void OnModUnequipped()
    {
        base.OnModUnequipped();

        PlayerController.Instance.playerWeapon.bulletSpeedMultiplier /= speedMultiplier;
    }
}
