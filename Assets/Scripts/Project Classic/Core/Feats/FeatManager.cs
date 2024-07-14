using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Called when an feat is unlocked. Used primarily for notifications.
/// </summary>
public class FeatUnlockedEvent
{
    public BaseFeat feat;

    public FeatUnlockedEvent(BaseFeat _feat)
    {
        feat = _feat;
    }
}

/// <summary>
/// Called when an feat has progress to indicate. Used primarily for notifications.
/// </summary>
public class FeatProgressIndicatedEvent
{
    public BaseFeat feat;
    public int currentProgress;
    public int maxProgress;

    public FeatProgressIndicatedEvent(BaseFeat _feat, int _current, int _max)
    {
        feat = _feat;
        currentProgress = _current;
        maxProgress = _max;
    }
}

/// <summary>
/// A class that manages the feats in the game.
/// Designed as a singleton that persists.
/// </summary>
public class FeatManager : MonoBehaviour
{
    public static FeatManager instance;
    public List<BaseFeat> feats;
    private Dictionary<string, BaseFeat> featsByName;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        featsByName = new Dictionary<string, BaseFeat>();
        foreach (BaseFeat feat in feats)
        {
            feat.Initialize();
            feat.SetUnlockWithoutNotify(false);
            featsByName.Add(feat.name, feat);
        }
    }

    public BaseFeat GetFeatByName(string name)
    {
        if (featsByName.ContainsKey(name)) 
            return featsByName[name];

        return null;
    }
}
