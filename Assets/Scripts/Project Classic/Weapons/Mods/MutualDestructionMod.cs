using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mutual Destruction Mod", menuName = "Project Classic/Mods/Mutual Destruction Mod")]
public class MutualDestructionMod : WeaponMod
{
    public float multiplier;

    public override void OnModEquipped()
    {
        base.OnModEquipped();
        PlayerController.Instance.playerHealth.armor -= 1;
    }

    public override void OnModUnequipped()
    {
        base.OnModUnequipped();
        PlayerController.Instance.playerHealth.armor += 1;
    }

    public override void Apply(Projectile projectile)
    {
        base.Apply(projectile);

        projectile.damage = (int)(projectile.damage * multiplier);
    }
}
