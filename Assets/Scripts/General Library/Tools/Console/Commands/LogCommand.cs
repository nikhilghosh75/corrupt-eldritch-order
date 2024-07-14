using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Logs a given command to the Unity console
 * Written by WIlliam Bostick '20, Brandon Schulz '22
 */

namespace WSoft.Tools.Console
{
    public class LogCommand : ConsoleCommand
    {
        public LogCommand()
        {
            commandWord = "log";
        }

        public override bool Process(string[] args)
        {
            string logText = string.Join(" ", args);

            Debug.Log(logText);

            return true;
        }

        public override List<string> GetValidArgs() { return new List<string>(); }
    }
}