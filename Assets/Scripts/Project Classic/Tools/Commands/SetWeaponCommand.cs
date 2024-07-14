using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using WSoft.Tools.Console;

public class SetWeaponCommand : ConsoleCommand
{
    PlayerWeapon playerWeapon;

    public SetWeaponCommand()
    {
        commandWord = "setWeapon";
        playerWeapon = GameObject.FindObjectOfType<PlayerWeapon>();
    }

    public override List<string> GetValidArgs()
    {
        WeaponDatabase database = Resources.Load<WeaponDatabase>("All Weapons");

        List<string> args = new List<string>();
        for (int i = 0; i < database.weapons.Count; i++)
        {
            args.Add(database.weapons[i].name);
        }

        return args;
    }

    public override bool Process(string[] args)
    {
        string errorMessage = "Invalid usage of 'setWeapon'. Expected usage: 'setWeapon [valid weapon]'";

        if (args.Length != 1) 
        {
            Debug.LogError(errorMessage);
            return false;
        }

        WeaponDatabase database = Resources.Load<WeaponDatabase>("All Weapons");
        WeaponDatabase.Data data = database.weapons.Find(x => x.name.ToLower() == args[0]);

        if (data != null)
        {
            playerWeapon.SetWeapon(data.soweapon);
            EventBus.Publish<WeaponPowerupEvent>(new WeaponPowerupEvent(new SOWeaponPowerup(data.soweapon.weaponIcon, data.soweapon), playerWeapon));
            return true;
        }

        return true;
    }
}
