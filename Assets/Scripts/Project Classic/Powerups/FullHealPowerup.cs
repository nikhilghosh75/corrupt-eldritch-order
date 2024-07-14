using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Full Health", menuName = "Project Classic/Powerups/Full Heal Powerup")]
public class FullHealPowerup : SOPowerup
{
    public override bool ApplyPowerup(PlayerPowerupManager target)
    {
        target.playerHealth.Heal(target.playerHealth.maxHealth - target.playerHealth.Current);
        return true;
    }
}
