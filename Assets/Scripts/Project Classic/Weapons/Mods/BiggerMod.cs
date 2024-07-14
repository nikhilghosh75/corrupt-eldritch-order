using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For Bigger is Nastier

[CreateAssetMenu(fileName = "New Bigger Mod", menuName = "Project Classic/Mods/Bigger Mod")]
public class BiggerMod : WeaponMod
{
    public float conversionFactor;

    public override void Apply(Projectile projectile)
    {
        base.Apply(projectile);

        float multiplier = PlayerController.Instance.playerWeapon.bulletSizeMultiplier;
        projectile.damage = (int) (projectile.damage * Mathf.Max(multiplier * conversionFactor, 1f));

        EventBus.Publish(new ModActivatedEvent(this));
    }
}
