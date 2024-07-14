using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Jump Mod", menuName = "Project Classic/Mods/Jump Mod")]
public class JumpMod : WeaponMod
{
    Subscription<JumpEvent> jump_event_subscription;

    public override void OnModEquipped()
    {
        base.OnModEquipped();
        PlayerController.Instance.airController.maxExtraJumps += 1;
        jump_event_subscription = EventBus.Subscribe<JumpEvent>(OnJump);
    }

    public override void OnModUnequipped()
    {
        base.OnModUnequipped();
        PlayerController.Instance.airController.maxExtraJumps -= 1;
        EventBus.Unsubscribe(jump_event_subscription);
    }

    void OnJump(JumpEvent e)
    {
        EventBus.Publish(new ModActivatedEvent(this));
    }
}
