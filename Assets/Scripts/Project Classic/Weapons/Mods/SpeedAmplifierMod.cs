using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For Speed Amplifier

[CreateAssetMenu(fileName = "New Speed Amplifier Mod", menuName = "Project Classic/Mods/Speed Amplifier Mod")]
public class SpeedAmplifierMod : WeaponMod
{
    public float conversionFactor;

    public override void Apply(Projectile projectile)
    {
        base.Apply(projectile);

        float multiplier = PlayerController.Instance.groundController.GroundSpeedMultiplier;
        projectile.damage = (int) (projectile.damage * Mathf.Max(multiplier * conversionFactor, 1f));
    }
}
