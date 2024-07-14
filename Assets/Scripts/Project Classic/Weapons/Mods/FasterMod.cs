using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For Faster is Nastier

[CreateAssetMenu(fileName = "New Faster Mod", menuName = "Project Classic/Mods/Faster Mod")]
public class FasterMod : WeaponMod
{
    public float conversionFactor;

    public override void Apply(Projectile projectile)
    {
        base.Apply(projectile);

        float multiplier = PlayerController.Instance.playerWeapon.bulletSpeedMultiplier;
        projectile.damage = (int) (projectile.damage * Mathf.Max(multiplier * conversionFactor, 1f));

        EventBus.Publish(new ModActivatedEvent(this));
    }
}
