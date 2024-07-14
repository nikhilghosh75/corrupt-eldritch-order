using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Freeze Mod", menuName = "Project Classic/Mods/Freeze Mod")]
public class FreezeMod : WeaponMod
{
    public float chanceToFreeze = 0.1f;
    public float freezeDuration = 2f;

    Subscription<EnemyFrozenEvent> enemy_frozen_event;

    public override void OnModEquipped()
    {
        base.OnModEquipped();
        enemy_frozen_event = EventBus.Subscribe<EnemyFrozenEvent>(OnEnemyFrozen);
    }

    public override void OnModUnequipped()
    {
        base.OnModUnequipped();
        EventBus.Unsubscribe(enemy_frozen_event);
    }

    public override void Apply(Projectile projectile)
    {
        base.Apply(projectile);

        if (Random.Range(0f, 1f) < chanceToFreeze)
        {
            projectile.applyFreeze = true;
            projectile.freezeTime = freezeDuration;
        }
    }

    void OnEnemyFrozen(EnemyFrozenEvent e)
    {
        EventBus.Publish(new ModActivatedEvent(this));
    }
}
