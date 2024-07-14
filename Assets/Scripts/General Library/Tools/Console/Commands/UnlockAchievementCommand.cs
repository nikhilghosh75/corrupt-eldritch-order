using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Achievements;

/*
 * Unlocks a specific achievement. Can take in an integer for the ID or a string for the steam ID
 * Written by Howard Yang '24
 */

namespace WSoft.Tools.Console
{
    public class UnlockAchievementCommand : ConsoleCommand
    {
        public UnlockAchievementCommand()
        {
            commandWord = "unlockachievement";
        }

        public override List<string> GetValidArgs()
        {
            List<string> validArgs = new List<string>();

            if (AchievementsManager.instance != null)
            {
                foreach (BaseAchievement acheivement in AchievementsManager.instance.achievements)
                    validArgs.Add(acheivement.SteamID);

                foreach (BaseAchievement acheivement in AchievementsManager.instance.achievements)
                    validArgs.Add(acheivement.ID.ToString());
            }

            return validArgs;
        }

        public override bool Process(string[] args)
        {
            if (args.Length != 1)
            {
                Debug.Log("(Unlock Achievement Command) Not Enough Arguments!");
                return false;
            }
            if (AchievementsManager.instance == null)
            {
                Debug.Log("(Unlock Achievement Command) Instance of AchievementsManager does not exist!");
                return false;
            }

            int id;
            if (int.TryParse(args[0], out id))
                AchievementsManager.instance.FindAchievement(id).SetUnlocked();
            else
                AchievementsManager.instance.FindAchievement(args[0].ToUpper()).SetUnlocked();

            return true;
        }
    }
}