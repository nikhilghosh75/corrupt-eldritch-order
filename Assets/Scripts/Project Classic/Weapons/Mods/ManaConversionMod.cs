using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mana Conversion Mod", menuName = "Project Classic/Mods/Mana Conversion Mod")]
public class ManaConversionMod : WeaponMod
{
    public float scale;

    public override void Apply(Projectile projectile)
    {
        base.Apply(projectile);

        PlayerMana playerMana = PlayerController.Instance.playerMana;
        float manaScaleFactor = (playerMana.currentMana / playerMana.maxMana) + 1;
        projectile.damage = (int)(projectile.damage * manaScaleFactor * scale);

        if (manaScaleFactor > 1.3f)
        {
            EventBus.Publish(new ModActivatedEvent(this));
        }
    }
}
