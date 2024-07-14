using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Mod", menuName = "Project Classic/Mods/Adrenaline Surge Mod")]
public class AdrenalineSurgeMod : WeaponMod
{
    public override void Apply(Projectile projectile)
    {
        base.Apply(projectile);
        PlayerHealth playerHealth = PlayerController.Instance.playerHealth;

        int healthLost = playerHealth.maxHealth - playerHealth.Current;
        projectile.damage = (int)(projectile.damage * 3f * ((float)healthLost / (float)playerHealth.maxHealth));

        EventBus.Publish(new ModActivatedEvent(this));
    }
}
