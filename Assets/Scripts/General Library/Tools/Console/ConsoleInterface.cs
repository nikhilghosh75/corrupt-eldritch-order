using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * An interface for all things that can be executed by the console
 * Right now it's just limited to just console commands, but can be expanded to other things
 * Written by Brandon Schulz '22, William Bostick '20
 */

namespace WSoft.Tools.Console
{
    public interface ConsoleInterface
    {
        string CommandWord { get; }
        bool Process(string[] args);
        List<string> AutoComplete(string[] args);
        List<string> GetValidArgs();
    }
}