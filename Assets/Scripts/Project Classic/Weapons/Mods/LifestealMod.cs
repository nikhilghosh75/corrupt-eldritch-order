using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Lifesteal Mod", menuName = "Project Classic/Mods/Lifesteal Mod")]
public class LifestealMod : WeaponMod
{
    public float lifestealChance;

    Subscription<EnemyKilledEvent> enemyKilledEvent;

    public override void OnModEquipped()
    {
        base.OnModEquipped();

        enemyKilledEvent = EventBus.Subscribe<EnemyKilledEvent>(OnEnemyKilled);
    }

    public override void OnModUnequipped()
    {
        base.OnModUnequipped();
        EventBus.Unsubscribe(enemyKilledEvent);
    }

    void OnEnemyKilled(EnemyKilledEvent e)
    {
        if (Random.Range(0f, 1f) < lifestealChance)
        {
            PlayerController.Instance.playerHealth.Heal(1);
            EventBus.Publish(new ModActivatedEvent(this));
        }
    }
}
