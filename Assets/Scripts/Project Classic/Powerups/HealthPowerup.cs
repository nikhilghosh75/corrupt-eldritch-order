using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPowerup", menuName = "Project Classic/Powerups/HealthPowerup")]

public class HealthPowerup : SOPowerup
{
    public int health;

    public override bool ApplyPowerup(PlayerPowerupManager target)
    {
        target.playerHealth.Heal(health);
        return base.ApplyPowerup(target);
    }
}
