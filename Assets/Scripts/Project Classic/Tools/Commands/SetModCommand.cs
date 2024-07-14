using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Tools.Console;

public class SetModCommand : ConsoleCommand
{
    public SetModCommand()
    {
        commandWord = "setmod";
    }

    public override List<string> GetValidArgs()
    {
        List<string> args = new List<string>();

        ModDatabase database = Resources.Load<ModDatabase>("All Mods");
        for (int i = 0; i < database.mods.Count; i++)
        {
            string modName = database.mods[i].displayName;
            string commandName = modName.ToLower().Replace(" ", "");

            args.Add("weak " + commandName);
            args.Add("strong " + commandName);
        }

        return args;
    }

    public override bool Process(string[] args)
    {
        if (args.Length != 2)
        {
            return false;
        }
        if (args[0] != "strong" && args[0] != "weak")
        {
            return false;
        }

        PlayerWeapon playerWeapon = GameObject.FindObjectOfType<PlayerWeapon>();

        ModDatabase database = Resources.Load<ModDatabase>("All Mods");
        for (int i = 0; i < database.mods.Count; i++)
        {
            string modName = database.mods[i].displayName;
            string commandName = modName.ToLower().Replace(" ", "");

            if (commandName == args[1])
            {
                if (args[0] == "strong")
                {
                    playerWeapon.AddMod(WeaponType.Strong, database.mods[i]);
                }
                else if (args[0] == "weak")
                {
                    playerWeapon.AddMod(WeaponType.Weak, database.mods[i]);
                }
                return true;
            }
        }

        return false;
    }
}
