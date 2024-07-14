using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpHeightPowerup", menuName = "Project Classic/Powerups/JumpHeightPowerUp")]


public class JumpHeightPowerUp : SOPowerup
{
    public float jumpMultiplier;

    public override bool ApplyPowerup(PlayerPowerupManager target)
    {
        target.playerSpeed.airController.ChangeJumpStrength(jumpMultiplier);
        return base.ApplyPowerup(target);
    }
}
