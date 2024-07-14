using System.Collections.Generic;
using UnityEngine;
using WSoft.Tools.Console;

public class InvincibleCommand : ConsoleCommand
{
    public InvincibleCommand()
    {
        commandWord = "SetInvincible";
    }

    public override List<string> GetValidArgs()
    {
        return new List<string>() { "on", "off" };
    }

    public override bool Process(string[] args)
    {
        string errorMessage = "Invalid usage of 'SetInvincible'. Expected usage: 'SetInvincible [on/off]'";

        PlayerInvincibilityController invincibilityController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInvincibilityController>();

        if (args.Length != 1) 
        {
            Debug.LogError(errorMessage);
            return false;
        }

        if (args[0] == "on")
        {
            invincibilityController.ApplyInvincibilityPermanent(on: true);
        }
        else if (args[0] == "off")
        {
            invincibilityController.ApplyInvincibilityPermanent(on: false);
        }
        else
        {
            Debug.LogError(errorMessage);
            return false;
        }

        return true;
    }
}
