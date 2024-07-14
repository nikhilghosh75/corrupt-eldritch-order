using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Vitality Mod", menuName = "Project Classic/Mods/Vitality Surge Mod")]
public class VitalitySurgeMod : WeaponMod
{
    public override void Apply(Projectile projectile)
    {
        base.Apply(projectile);

        PlayerHealth playerHealth = PlayerController.Instance.playerHealth;
        projectile.damage = (int)(projectile.damage * 2f * ((float)playerHealth.Current / (float)playerHealth.maxHealth));

        EventBus.Publish(new ModActivatedEvent(this));
    }
}
