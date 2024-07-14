using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if WSOFT_STEAM
using Steamworks;
#endif

/*
 * The manager of achievements, along with the event classes that handle them
 * Written by Howard Yang '24, Nikhil Ghosh '24
 */

namespace WSoft.Achievements
{
    /// <summary>
    /// Called when the achievements are cleared from the achievements manager. Useful for achievement displays
    /// </summary>
    public class AchievementsClearedEvent { }

    /// <summary>
    /// Called when any update to achievement unlocking is called. Enables game to update achievement displays
    /// </summary>
    public class AchievementsUpdatedEvent { }

    /// <summary>
    /// Called when an achievement is unlocked. Used primarily for notifications
    /// </summary>
    public class AchievementUnlockedEvent
    {
        public BaseAchievement achievement;

        public AchievementUnlockedEvent(BaseAchievement _achievement)
        {
            achievement = _achievement;
        }
    }

    /// <summary>
    /// Called when an achievement has progress to indicate. Used primarily for notifications
    /// </summary>
    public class AchievementProgressIndicatedEvent
    {
        public BaseAchievement achievement;
        public int currentProgress;
        public int maxProgress;

        public AchievementProgressIndicatedEvent(BaseAchievement _achievement, int _current, int _max)
        {
            achievement = _achievement;
            currentProgress = _current;
            maxProgress = _max;
        }
    }

    /// <summary>
    /// A class that manages the achievements in the game.
    /// Designed to go on an achievement manager and be persistent throughout the entire game
    /// </summary>
    public class AchievementsManager : MonoBehaviour
    {
        public static AchievementsManager instance;

        public List<BaseAchievement> achievements;

        Dictionary<string, BaseAchievement> achievementByApiKey = new Dictionary<string, BaseAchievement>();
        Dictionary<int, BaseAchievement> achievementByID = new Dictionary<int, BaseAchievement>();

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            for (int i = 0; i < achievements.Count; i++)
            {
                achievements[i].Initialize();
                achievementByApiKey.Add(achievements[i].SteamID, achievements[i]);
                achievementByID.Add(achievements[i].ID, achievements[i]);
            }
        }

        public BaseAchievement FindAchievement(string apiKey)
        {
            if (achievementByApiKey.ContainsKey(apiKey))
                return achievementByApiKey[apiKey];
            return null;
        }

        public BaseAchievement FindAchievement(int id)
        {
            if (achievementByID.ContainsKey(id))
                return achievementByID[id];
            return null;
        }

        public void ClearAchievements()
        {
            foreach (BaseAchievement achievement in achievements)
            {
                achievement.SetUnlockedWithoutNotify(false);
            }

#if WSOFT_STEAM
            SteamUserStats.ResetAllStats(true);
#endif
            EventBus.Publish(new AchievementsClearedEvent());
            EventBus.Publish(new AchievementsUpdatedEvent());
        }
    }
}
