using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if WSOFT_STEAM
using Steamworks;
#endif

namespace WSoft.Achievements
{
    /// <summary>
    /// A base achievement class that is meant to be inherited.
    /// Inherited classes implement Initialize to subscribe to events and callbacks
    /// Nikhil Ghosh '24
    /// </summary>
    public abstract class BaseAchievement : ScriptableObject
    {
        public int ID;
        public string SteamID;
        public string Name;
        public string Description;

        [HideInInspector]
        public bool IsUnlocked { get; private set; }

        public abstract void Initialize();

        public void SetUnlocked()
        {
            if (!IsUnlocked)
                Unlock();
        }

        public void SetUnlockedWithoutNotify(bool isUnlocked)
        {
            IsUnlocked = isUnlocked;
        }

        protected void Unlock()
        {
            if (IsUnlocked)
                return;

            IsUnlocked = true;

#if WSOFT_STEAM
            if(SteamManager.Initialized)
            {
                SteamUserStats.SetAchievement(SteamID);
                SteamUserStats.StoreStats();
            }
#endif

            Debug.Log("Achievement Unlocked: " + name);
            EventBus.Publish(new AchievementUnlockedEvent(this));
            EventBus.Publish(new AchievementsUpdatedEvent());
        }

        protected void IndicateAchievementProgress(int CurrentProgress, int MaxProgress)
        {
            EventBus.Publish(new AchievementProgressIndicatedEvent(this, CurrentProgress, MaxProgress));

#if WSOFT_STEAM
            if(SteamManager.Initialized)
            {
                SteamUserStats.IndicateAchievementProgress(SteamID, (uint)CurrentProgress, (uint)MaxProgress);
            }
#endif
        }
    }

}