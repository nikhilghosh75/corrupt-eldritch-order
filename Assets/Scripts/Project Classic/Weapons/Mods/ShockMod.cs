using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shock Mod", menuName = "Project Classic/Mods/Shock Mod")]
public class ShockMod : WeaponMod
{

    public override void Apply(Projectile projectile)
    {
        base.Apply(projectile);

        projectile.applyShock = true;
    }
}
