using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletSpeedPowerup", menuName = "Project Classic/Powerups/BulletSpeedPowerUp")]


public class BulletSpeedPowerUp : SOPowerup
{
    public float speedMultiplier;

    public override bool ApplyPowerup(PlayerPowerupManager target)
    {
        target.playerWeapon.bulletSpeedMultiplier *= speedMultiplier;
        return base.ApplyPowerup(target);
    }
}
