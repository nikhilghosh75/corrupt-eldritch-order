using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedUpPowerup", menuName = "Project Classic/Powerups/SpeedUpPowerUp")]


public class SpeedUpPowerUp : SOPowerup
{
    public float speedMultiplier;

    public override bool ApplyPowerup(PlayerPowerupManager target)
    {
        target.playerSpeed.groundController.ChangeSpeed(speedMultiplier);
        return base.ApplyPowerup(target);
    }
}
