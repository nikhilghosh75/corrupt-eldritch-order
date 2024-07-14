using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using WSoft.Tools.Console;

public class ClassChangeCommand : ConsoleCommand
{
    public ClassChangeCommand()
    {
        commandWord = "changeclass";
    }

    public override List<string> GetValidArgs()
    {
        return new List<string>() {"base", "ninja", "wizard", "tank"};
    }

    public override bool Process(string[] args)
    {
        string errorMessage = "Invalid usage of 'ChangeClass'. Expected usage: 'changeclass [base | ninja | wizard | tank]'";

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        PlayerClassBase baseClass = player.GetComponent<PlayerClassBase>();
        PlayerClassNinja ninja = player.GetComponent<PlayerClassNinja>();
        PlayerClassWizard wizard = player.GetComponent<PlayerClassWizard>();
        PlayerClassTank tank = player.GetComponent<PlayerClassTank>();

        if (args.Length != 1)
        {
            Debug.LogError(errorMessage);
            return false;
        }
        else
        {
            baseClass.enabled = false;
            ninja.enabled = false;
            wizard.enabled = false;
            tank.enabled = false;

            AnimationCharacterController PlayerAnimation = player.GetComponentInChildren<AnimationCharacterController>();

            switch (args[0])
            {
                case "base":
                    baseClass.enabled = true;
                    PlayerAnimation.ActivateCharacterGraphic(CharacterType.Default);
                    break;
                case "ninja":
                    ninja.enabled = true;
                    PlayerAnimation.ActivateCharacterGraphic(CharacterType.Ninja);
                    break;
                case "wizard":
                    wizard.enabled = true;
                    PlayerAnimation.ActivateCharacterGraphic(CharacterType.Wizard);
                    break;
                case "tank":
                    tank.enabled = true;
                    PlayerAnimation.ActivateCharacterGraphic(CharacterType.Tank);
                    break;
            }


        }

        return true;
    }
}
