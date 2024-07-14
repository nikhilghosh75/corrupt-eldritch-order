using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOPowerup : ScriptableObject
{
    public string displayName;
    public int cost;

    public virtual bool ApplyPowerup(PlayerPowerupManager target) {

        EventBus.Publish<PowerupEvent>(new PowerupEvent(this));

        return true;
    }

}