using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Health Conversion Mod", menuName = "Project Classic/Mods/Health Conversion Mod")]
public class HealthConversionMod : WeaponMod
{
    public float manaMultiplier = 1.5f;

    public override void OnModEquipped()
    {
        base.OnModEquipped();
        PlayerController.Instance.playerHealth.Damage(1);
        PlayerController.Instance.playerMana.manaGenerationMultiplier *= manaMultiplier;
    }

    public override void OnModUnequipped()
    {
        base.OnModUnequipped();
        PlayerController.Instance.playerHealth.Heal(1);
        PlayerController.Instance.playerMana.manaGenerationMultiplier /= manaMultiplier;
    }


}
