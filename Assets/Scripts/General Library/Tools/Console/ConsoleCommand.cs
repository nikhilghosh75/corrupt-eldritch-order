using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * An abstract class representing a console command
 * Written by Brandon Schulz '22, William Bostick '20
 */

namespace WSoft.Tools.Console
{
    public abstract class ConsoleCommand : ConsoleInterface
    {
        [SerializeField] protected string commandWord = string.Empty;

        public string CommandWord => commandWord;

        public abstract bool Process(string[] args);
        public abstract List<string> GetValidArgs();
        public virtual List<string> AutoComplete(string[] args)
        {
            List<string> matchingCommands = new List<string>();
            if (args.Length > 1) { return new List<string>(); }

            foreach (string validArg in GetValidArgs())
            {
                if (validArg.ToLower().StartsWith(args[0].ToLower())) { matchingCommands.Add(validArg.ToLower()); }
            }

            return matchingCommands;
        }
    }
}