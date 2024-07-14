using UnityEngine;

/// <summary>
/// A base class for feats. Implement Initialize to subscribe to events and callbacks.
/// </summary>
public abstract class BaseFeat : ScriptableObject
{
    public string Name;
    public string Description;
    public float priority;

    [Header("Rewards")]
    public SOWeapon weapon;
    public string className;

    private bool IsUnlocked;

    public bool Unlocked => IsUnlocked;

    public abstract void Initialize();

    public void SetUnlockWithoutNotify(bool _unlocked)
    {
        IsUnlocked = _unlocked;
    }

    public void Unlock()
    {
        if (IsUnlocked)
            return;

        IsUnlocked = true;

        EventBus.Publish(new FeatUnlockedEvent(this));

        if (weapon != null)
        {
            RunManager.Instance.unlockedWeapons.Add(weapon);
        }

        if (className != "")
        {
            RunManager.Instance.unlockedClasses.Add(className);
        }

        RunManager.Instance.SavePermanentProgress();
    }

    public abstract float GetProgress();
    public abstract string GetProgressText();
    public abstract string GetProgressTextAtProgress(float progress);

    public string RewardString()
    {
        if (weapon != null)
        {
            WeaponDatabase database = Resources.Load<WeaponDatabase>("All Weapons");
            return database.weapons.Find(w => w.soweapon == weapon).name;
        }

        if (className != "")
        {
            return className;
        }

        return "";
    }

    /// <summary>
    /// Resets the feat progress
    /// </summary>
    public void ResetProgress(int MaxProgress)
    {
        IsUnlocked = false;
        EventBus.Publish(new FeatProgressIndicatedEvent(this, 0, MaxProgress));
    }

    protected void IndicateFeatProgress(int CurrentProgress, int MaxProgress)
    {
        EventBus.Publish(new FeatProgressIndicatedEvent(this, CurrentProgress, MaxProgress));
    }
}
