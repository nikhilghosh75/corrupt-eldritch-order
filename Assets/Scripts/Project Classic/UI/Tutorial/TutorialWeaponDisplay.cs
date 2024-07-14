using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWeaponDisplay : MonoBehaviour
{
    public SpriteRenderer weakWeaponIndicator;
    public SpriteRenderer strongWeaponIndicator;
    public SpriteRenderer comboWeaponIndicator;

    public Sprite fullSprite;
    public Sprite emptySprite;

    public LevelExit door;

    Subscription<WeaponFiredEvent> weapon_fired;
    Subscription<ComboWeaponStartedEvent> combo_weapon_started;

    // Start is called before the first frame update
    void Start()
    {
        weakWeaponIndicator.sprite = emptySprite;
        strongWeaponIndicator.sprite = emptySprite;
        comboWeaponIndicator.sprite = emptySprite;

        weapon_fired = EventBus.Subscribe<WeaponFiredEvent>(OnWeaponFired);
        combo_weapon_started = EventBus.Subscribe<ComboWeaponStartedEvent>(OnComboWeaponStarted);

        door.Add();
    }

    void OnWeaponFired(WeaponFiredEvent e)
    {
        if (e.weapon.type == WeaponType.Weak)
        {
            weakWeaponIndicator.sprite = fullSprite;
        }
        else if (e.weapon.type == WeaponType.Strong)
        {
            strongWeaponIndicator.sprite = fullSprite;
        }
        else if (e.weapon.type == WeaponType.Combo)
        {
            comboWeaponIndicator.sprite = fullSprite;
        }
        CheckDoorOpen();
    }

    void OnComboWeaponStarted(ComboWeaponStartedEvent e)
    {
        comboWeaponIndicator.sprite = fullSprite;
        CheckDoorOpen();
    }

    void CheckDoorOpen()
    {
        if (weakWeaponIndicator.sprite == fullSprite
            && strongWeaponIndicator.sprite == fullSprite
            && comboWeaponIndicator.sprite == fullSprite)
        {
            door.Remove();
            EventBus.Unsubscribe(weapon_fired);
        }
    }
}
