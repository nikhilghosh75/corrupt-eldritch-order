using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Mod", menuName = "Project Classic/Mods/Health Mod")]
public class HealthMod : WeaponMod
{
    public int healthAmount;

    public override void OnModEquipped()
    {
        base.OnModEquipped();

        PlayerController.Instance.playerHealth.IncreaseMaxHealth(healthAmount);
    }

    public override void OnModUnequipped()
    {
        base.OnModUnequipped();

        PlayerController.Instance.playerHealth.DecreaseMaxHealth(healthAmount);
    }
}
