using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MaxHealthPowerup", menuName = "Project Classic/Powerups/MaxHealthPowerup")]

public class MaxHealthPowerup : SOPowerup
{
    public float maxHealthMultiplier;
    public int health;

    public override bool ApplyPowerup(PlayerPowerupManager target)
    {


        target.playerHealth.ChangeMaxHealth(maxHealthMultiplier);
        return target.playerHealth.Heal(health);
    }
}
