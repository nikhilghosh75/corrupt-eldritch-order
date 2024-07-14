using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShieldPowerup", menuName = "Project Classic/Powerups/ShieldPowerup")]

public class ShieldPowerup : SOPowerup
{
    public int shield = 2;

    public override bool ApplyPowerup(PlayerPowerupManager target) //Can't just heal 2 HP so I have to do something a bit annoying
    {
        int curMaxHealth = target.playerHealth.maxHealth; //store "real" max health
        target.playerHealth.SetMaxAndHeal(shield + target.playerHealth.Current); //set max health to be 2 higher than current health to allow "overhealing", then heal to "max"
        target.playerHealth.SetMaxHealth(curMaxHealth); //restore "real" max health
        return base.ApplyPowerup(target);
    }
}
