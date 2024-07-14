using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageUpPowerup", menuName = "Project Classic/Powerups/DamageUpPowerUp")]
public class DamageUpPowerUp : SOPowerup
{
    public float damageMultiplier;

    public override bool ApplyPowerup(PlayerPowerupManager target)
    {
        target.playerWeapon.damageMultiplier *= 2;
        return base.ApplyPowerup(target);
    }
}
