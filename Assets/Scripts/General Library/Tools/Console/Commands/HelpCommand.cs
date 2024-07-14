using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Prints all the availible commands
 * Written by Brandon Schulz '22, Matt Rader '19, William Bostick '20
 */

namespace WSoft.Tools.Console
{
    public class HelpCommand : ConsoleCommand
    {
        public HelpCommand()
        {
            commandWord = "help";
        }

        public override bool Process(string[] args)
        {
            Debug.Log("\n/changescene <string> :  changes the scene to the desired scene (currently only MainMenu)");
            Debug.Log("\n/log <string> : logs a string in the console");
            return true;
        }

        public override List<string> GetValidArgs() { return new List<string>(); }
    }
}