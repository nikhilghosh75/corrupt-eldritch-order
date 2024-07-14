using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fire Mod", menuName = "Project Classic/Mods/Fire Mod")]
public class FireMod : WeaponMod
{
    public float burnTimeBetweenDamages;
    public int numDamagesForBurn;

    Subscription<EnemyBurnedEvent> burned_event_subscription;

    public override void OnModEquipped()
    {
        base.OnModEquipped();

        burned_event_subscription = EventBus.Subscribe<EnemyBurnedEvent>(OnEnemyBurned);
    }

    public override void OnModUnequipped()
    {
        base.OnModUnequipped();

        EventBus.Unsubscribe(burned_event_subscription);
    }

    public override void Apply(Projectile projectile)
    {
        base.Apply(projectile);

        projectile.applyBurn = true;
        projectile.numBurns = numDamagesForBurn;
        projectile.timeBetweenBurns = burnTimeBetweenDamages;
    }

    void OnEnemyBurned(EnemyBurnedEvent e)
    {
        EventBus.Publish(new ModActivatedEvent(this));
    }
}
