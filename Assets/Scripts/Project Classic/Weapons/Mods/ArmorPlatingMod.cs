using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Plating Mod", menuName = "Project Classic/Mods/Armor Plating Mod")]
public class ArmorPlatingMod : WeaponMod
{
    public override void OnModEquipped()
    {
        base.OnModEquipped();
        PlayerController.Instance.playerHealth.armor += 1;
    }

    public override void OnModUnequipped()
    {
        base.OnModUnequipped();
        PlayerController.Instance.playerHealth.armor -= 1;
    }
}
