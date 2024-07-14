using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InvincibilityPowerup", menuName = "Project Classic/Powerups/InvincibilityPowerup")]

public class InvincibilityPowerup : SOPowerup
{
    public float duration;

    public override bool ApplyPowerup(PlayerPowerupManager target)
    {
        target.invincibilityController.ApplyInvincibility(duration);
        base.ApplyPowerup(target);
        return true;
    }
}
