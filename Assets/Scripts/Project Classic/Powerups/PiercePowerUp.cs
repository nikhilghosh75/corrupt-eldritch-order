using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PiercePowerup", menuName = "Project Classic/Powerups/PiercePowerUp")]


public class PiercePowerUp : SOPowerup
{
    public int numExtraPierces;

    public override bool ApplyPowerup(PlayerPowerupManager target)
    {
        target.playerWeapon.numExtraPierces += numExtraPierces;
        return base.ApplyPowerup(target);
    }
}
