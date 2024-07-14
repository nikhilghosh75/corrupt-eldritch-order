using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/*
 * A class containing the functionality of a console
 * Intended to be separate from the mono-behaviour that controls it
 * Written by Brandon Schulz '22, William Bostick '20, Nikhil Ghosh '24
 */

namespace WSoft.Tools.Console
{
    public class DeveloperConsole
    {
        private readonly IEnumerable<ConsoleInterface> commands;

        /// <summary>
        /// Sets up a developer console with an instance of every command availible in the entire project
        /// </summary>
        public DeveloperConsole()
        {
            this.commands = GetAllCommands();
        }

        /// <summary>
        /// Sets up a developer console with an explicit list of commands.
        /// Useful if you only want a specific set of commands
        /// </summary>
        /// <param name="commands">The list of commands to set it to</param>
        public DeveloperConsole(IEnumerable<ConsoleInterface> commands)
        {
            this.commands = commands;
        }

        public void ProcessCommand(string inputValue)
        {
            string[] inputSplit = SplitInput(inputValue);

            string commandInput = inputSplit[0];
            string[] args = inputSplit.Skip(1).ToArray();
            ProcessCommand(commandInput, args);
        }

        public void ProcessCommand(string commandInput, string[] args)
        {
            foreach (var command in commands)
            {
                if (!commandInput.Equals(command.CommandWord, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (command.Process(args))
                {
                    return;
                }
            }
        }

        public List<string> AutoComplete(string inputValue)
        {
            string[] inputSplit = inputValue.Split(' ');
            string commandInput = inputSplit[0];

            List<string> matchingCommands = new List<string>();

            if (inputSplit.Length == 1)
            {

                foreach (var command in commands)
                {
                    if (command.CommandWord.StartsWith(commandInput))
                    {
                        matchingCommands.Add(command.CommandWord);
                    }
                }

                return matchingCommands;
            }
            else
            {
                string[] args = inputSplit.Skip(1).ToArray();

                foreach (var command in commands)
                {
                    if (command.CommandWord == commandInput)
                    {
                        List<string> matchingArgs = command.AutoComplete(args);
                        foreach (string commandArg in matchingArgs)
                        {
                            matchingCommands.Add(command.CommandWord + " " + commandArg);
                        }
                    }
                }

                return matchingCommands;
            }
        }

        /// <summary>
        /// Gets every type of ConsoleCommand in the project and instantiates it.
        /// </summary>
        /// <returns></returns>
        public List<ConsoleCommand> GetAllCommands()
        {
            List<ConsoleCommand> commands = new List<ConsoleCommand>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (type != null && type != typeof(ConsoleCommand))
                    {
                        if (typeof(ConsoleCommand).IsAssignableFrom(type))
                        {
                            ConsoleCommand command = (ConsoleCommand)Activator.CreateInstance(type);
                            commands.Add(command);
                        }
                    }
                }
            }

            return commands;
        }

        /// <summary>
        /// Splits the string while ignoring spaces within quotes
        /// e.g. "kill crew \"Smith Robertson\"" will return { "kill", "crew", "Smith Robertson" }
        /// </summary>
        string[] SplitInput(string input)
        {
            input = input.Trim();

            int lastSpaceIndex = 0;
            bool withinQuote = false;
            List<string> strs = new List<string>();
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == ' ')
                {
                    if (!withinQuote)
                    {
                        string arg = input.Substring(lastSpaceIndex, i - lastSpaceIndex).Replace("\"", string.Empty);
                        strs.Add(arg);
                        lastSpaceIndex = i + 1;
                    }
                }
                else if (input[i] == '"')
                {
                    withinQuote = !withinQuote;
                }
            }

            string lastArg = input.Substring(lastSpaceIndex).Replace("\"", string.Empty);
            strs.Add(lastArg);

            return strs.ToArray();
        }
    }
}