using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Mod", menuName = "Project Classic/Mods/Damage Mod")]
public class DamageMod : WeaponMod
{
    public float multiplier;

    public override void Apply(Projectile projectile)
    {
        base.Apply(projectile);

        projectile.damage = (int)(projectile.damage * multiplier);

        EventBus.Publish(new ModActivatedEvent(this));
    }
}
