using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cooldown Mod", menuName = "Project Classic/Mods/Cooldown Mod")]
public class AttackCooldownMod : WeaponMod
{
    public float attackCooldownMultiplier;

    public override void OnModEquipped()
    {
        base.OnModEquipped();
        PlayerController.Instance.playerWeapon.cooldownMultiplier *= attackCooldownMultiplier;
    }

    public override void OnModUnequipped()
    {
        base.OnModUnequipped();
        PlayerController.Instance.playerWeapon.cooldownMultiplier /= attackCooldownMultiplier;
    }
}
