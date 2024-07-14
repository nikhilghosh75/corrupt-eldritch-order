using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupTrigger : MonoBehaviour
{
    [SerializeField]
    public SOPowerup powerup;

    public bool ApplyPowerup(PlayerPowerupManager target)
    {
        return powerup.ApplyPowerup(target);
    }
}
