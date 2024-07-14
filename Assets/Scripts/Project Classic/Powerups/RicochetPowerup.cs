using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ricochet Powerup", menuName = "Project Classic/Powerups/RicochetPowerUp")]
public class RicochetPowerup : SOPowerup
{
    public override bool ApplyPowerup(PlayerPowerupManager target)
    {
        target.playerWeapon.addRicochet = true;

        return base.ApplyPowerup(target);
    }
}
