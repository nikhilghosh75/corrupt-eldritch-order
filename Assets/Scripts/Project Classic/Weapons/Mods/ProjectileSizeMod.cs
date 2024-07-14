using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Size Mod", menuName = "Project Classic/Mods/Projectile Size Mod")]
public class ProjectileSizeMod : WeaponMod
{
    public float SizeMultiplier;

    public override void OnModEquipped()
    {
        base.OnModEquipped();

        PlayerController.Instance.playerWeapon.bulletSizeMultiplier *= SizeMultiplier;
    }

    public override void OnModUnequipped()
    {
        base.OnModUnequipped();

        PlayerController.Instance.playerWeapon.bulletSizeMultiplier /= SizeMultiplier;
    }
}
