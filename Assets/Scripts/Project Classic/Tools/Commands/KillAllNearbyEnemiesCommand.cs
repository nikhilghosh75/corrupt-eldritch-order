using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Tools.Console;

public class KillAllNearbyEnemiesCommand : ConsoleCommand
{
    // Constructor
    public KillAllNearbyEnemiesCommand()
    {
        commandWord = "killNearbyEnemies";
    }

    // Valid Args
    public override List<string> GetValidArgs()
    {
        return new List<string>()
        {
            "10",
            "50",
            "100"
        };
    }

    // Functionality
    public override bool Process(string[] args)
    {
        // Check for valid args
        if (args.Length != 1)
        {
            Debug.LogError("killNearbyEnemies requires a distance as an argument");
            return false;
        }
        if (!float.TryParse(args[0], out float distance))
        {
            Debug.LogError("killNearbyEnemies requires a float as its first argument");
            return false;
        }

        PlayerController[] player = GameObject.FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        if (player.Length == 0)
        {
            Debug.LogError("No player object found in the scene!");
            return false;
        }
        else if (player.Length > 1)
        {
            Debug.LogError("Multiple player objects foudn in scene");
            return false;
        }

        EnemyHealth[] enemies = GameObject.FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);

        foreach (EnemyHealth enemy in enemies)
        {
            // Check if enemy is close to player
            if (Vector3.Distance(player[0].transform.position, enemy.transform.position) <= distance)
            {
                enemy.Destroy();
            }
        }

        return true;
    }
}
