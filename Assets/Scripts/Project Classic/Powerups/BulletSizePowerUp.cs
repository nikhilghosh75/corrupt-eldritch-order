using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletSizePowerup", menuName = "Project Classic/Powerups/BulletSizePowerUp")]


public class BulletSizePowerUp : SOPowerup
{
    public float sizeMultiplier;

    public override bool ApplyPowerup(PlayerPowerupManager target)
    {
        target.playerWeapon.bulletSizeMultiplier *= sizeMultiplier;
        return base.ApplyPowerup(target);
    }
}
