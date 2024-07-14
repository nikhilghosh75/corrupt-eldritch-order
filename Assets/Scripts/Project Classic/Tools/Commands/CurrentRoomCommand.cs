using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using WSoft.Tools.Console;

public class CurrentRoomCommand : ConsoleCommand
{
    private string currentRoomName;
    public CurrentRoomCommand()
    {
        commandWord = "GetCurrentRoom";
    }

    public override List<string> GetValidArgs()
    {
        return new List<string>() { };
    }

    public override bool Process(string[] args)
    {
        string errorMessage = "Invalid usage of 'GetCurrentRoom'. Expected usage: 'GetCurrentRoom'";

        PlayerCurrentRoom currentRoomScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCurrentRoom>();
        currentRoomName = currentRoomScript.currentRoomName;
        if (args.Length != 0) 
        {
            Debug.LogError(errorMessage);
            return false;
        }
        else
        {
            Debug.Log(currentRoomScript.currentRoomName);
        }

        return true;
    }
}
