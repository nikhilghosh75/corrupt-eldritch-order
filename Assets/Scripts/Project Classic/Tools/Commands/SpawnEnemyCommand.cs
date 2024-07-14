using System.Collections.Generic;
using UnityEngine;
using WSoft.Tools.Console;

public class SpawnCommand : ConsoleCommand
{
    GameObject player;
    public SpawnCommand()
    {
        commandWord = "spawnEnemy";
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public override List<string> GetValidArgs()
    {
        return new List<string>() { "Drone" };
    }

    public override bool Process(string[] args)
    {
        string errorMessage = "Invalid usage of 'spawnEnemy'. Expected usage: 'spawnEnemy [valid enemy]'";

        if (args.Length != 1) 
        {
            Debug.LogError(errorMessage);
            return false;
        }

        if (args[0] == "drone")
        {
            GameObject.Instantiate(Resources.Load("DroneEnemy"), new Vector3(player.transform.position.x, player.transform.position.y + 5, 0), Quaternion.identity);
        }
        else
        {
            Debug.LogError(errorMessage);
            return false;
        }

        return true;
    }
}
