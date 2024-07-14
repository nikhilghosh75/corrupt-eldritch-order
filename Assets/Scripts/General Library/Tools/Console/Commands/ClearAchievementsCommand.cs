using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Achievements;

/*
 * Clears all the achievements from the game
 * Nikhil Ghosh '24
 */

namespace WSoft.Tools.Console
{
    public class ClearAchievementsCommand : ConsoleCommand
    {
        public ClearAchievementsCommand()
        {
            commandWord = "clearachievements";
        }

        public override List<string> GetValidArgs() { return new List<string>(); }

        public override bool Process(string[] args)
        {

            if (AchievementsManager.instance == null)
            {
                Debug.Log("(Unlock Achievement Command) Instance of AchievementsManager does not exist!");
                return false;
            }

            AchievementsManager.instance.ClearAchievements();

            return true;
        }
    }
}