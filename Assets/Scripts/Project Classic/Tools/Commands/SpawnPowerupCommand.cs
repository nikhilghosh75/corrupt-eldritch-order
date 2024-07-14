using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Tools.Console;

public class SpawnPowerupCommand : ConsoleCommand
{
    public SpawnPowerupCommand()
    {
        commandWord = "spawnpowerup";
    }

    public override List<string> GetValidArgs()
    {
        PowerupDatabase database = Resources.Load<PowerupDatabase>("All Powerups");

        List<string> args = new List<string>();
        for(int i = 0; i < database.powerups.Count; i++)
        {
            args.Add(database.powerups[i].name);
        }

        return args;
    }

    public override bool Process(string[] args)
    {
        if (args.Length != 1)
        {
            return false;
        }

        PowerupDatabase database = Resources.Load<PowerupDatabase>("All Powerups");
        PowerupDatabase.Data data = database.powerups.Find(x => x.name == args[0]);

        if (data != null)
        {
            Vector3 location = PlayerController.Instance.transform.position + new Vector3(2, 0, 0);
            GameObject.Instantiate(data.prefab, location, Quaternion.identity);

            return true;
        }

        return false;
    }
}
