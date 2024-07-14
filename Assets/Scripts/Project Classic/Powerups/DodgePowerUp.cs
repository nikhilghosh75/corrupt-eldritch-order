using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DodgePowerup", menuName = "Project Classic/Powerups/DodgePowerUp")]
public class DodgePowerUp : SOPowerup
{
    public float dodgeSpeedMultiplier = 1.5f;

    public override bool ApplyPowerup(PlayerPowerupManager target)
    {
        target.playerSpeed.dashController.dashSpeed *= dodgeSpeedMultiplier;
        target.playerSpeed.dashController.attackDodge = true;
        return base.ApplyPowerup(target);
    }
}
