using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WSoft.Core;

public class CurrencyAddedEvent
{
    public int addedPermanentCurrency;
    public int addedRunCurrency;

    public CurrencyAddedEvent(int _permanent, int _run)
    {
        addedPermanentCurrency = _permanent;
        addedRunCurrency = _run;
    }
}

public enum CurrencyType
{
    Permanent,
    Run
}

public class ItemPurchasedEvent
{
    public int cost;
    public CurrencyType currencyType;
    public SOPowerup powerup;
    public UpgradeTypes upgradeType;

    public ItemPurchasedEvent(int _cost, SOPowerup _powerup)
    {
        cost = _cost;
        powerup = _powerup;
        currencyType = CurrencyType.Run;
    }

    public ItemPurchasedEvent(int _cost, UpgradeTypes _upgradeType)
    {
        cost = _cost;
        upgradeType = _upgradeType;
    }
}

public class RunManager : MonoBehaviour
{
    public static RunManager Instance { get; private set; }

    public int permanentCurrency { get; private set; }
    public int runCurrency { get; private set; }

    // Initial Player Variables
    public string selectedClassName { get; private set; }
    public SOWeapon initialLightWeapon { get; private set; }
    public SOWeapon initialStrongWeapon { get; private set; }

    public List<UpgradeTreeData> upgrades;

    public int luck = 0;
    [HideInInspector]
    public List<SOWeapon> unlockedWeapons;

    [HideInInspector]
    public List<string> unlockedClasses;

    // track the state of the boss across the course of the run
    public int[] currentAttackIDs;
    public int[] currentModeIDs;

    // The progress of each feat when the run started
    [HideInInspector]
    public Dictionary<string, float> featProgressAtStartOfRun = new Dictionary<string, float>();
    [HideInInspector]
    public ProgressionSaveData saveDataAtStartOfRun = new ProgressionSaveData();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else if (Instance != this)
        {
            //Instance is not the same as the one we have, destroy old one, and reset to newest one
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        EventBus.Subscribe<UpdateHealthEvent>(OnHealthChanged);

        unlockedWeapons = Resources.Load<WeaponDatabase>("All Weapons").GetAllDefaultUnlockedWeapons();

        unlockedClasses = new List<string>() { "Default" };
        LoadPermanentData();

        foreach(string _class in unlockedClasses)
        {
            Debug.Log(_class);
        }
    }

    void OnHealthChanged(UpdateHealthEvent e)
    {
        if (e.health + e.healthDelta <= 0)
        {
            SavePermanentProgress();
        }
    }

    public void ResetRun()
    {
        runCurrency = 0;

        featProgressAtStartOfRun.Clear();
        foreach (BaseFeat feat in FeatManager.instance.feats)
        {
            featProgressAtStartOfRun.Add(feat.Name, feat.GetProgress());
        }
        saveDataAtStartOfRun = GetPermanentProgress();

        SaveManager.Delete("InRunData");
    }

    public void AddCurrency(int permanent, int run)
    {
        permanentCurrency += permanent;
        runCurrency += run;
        EventBus.Publish(new CurrencyAddedEvent(permanent, run));
    }

    public void Purchase(int permanent, int run)
    {
        permanentCurrency -= permanent;
        runCurrency -= run;
    }

    // Allows setting the class for the player before they exist
    public void SetSelectedClassName(string name)
    {
        selectedClassName = name;
    }

    // Allows setting the starting weapons for the player before they exist
    public void SetStartingLightWeapon(SOWeapon weapon)
    {
        WeaponDatabase database = Resources.Load<WeaponDatabase>("All Weapons");
        WeaponDatabase.Data data = database.weapons.Find(x => x.soweapon == weapon); //Hopefully this doesn't break the search? - CS

        if (data != null && data.soweapon.type == WeaponType.Weak)
        {
            initialLightWeapon = data.soweapon;
        }
        else
        {
            initialLightWeapon = null; // used to reset starting weapons screen
        }
    }

    public void SetStartingStrongWeapon(SOWeapon weapon)
    {
        WeaponDatabase database = Resources.Load<WeaponDatabase>("All Weapons");
        WeaponDatabase.Data data = database.weapons.Find(x => x.soweapon == weapon); //Hopefully this doesn't break the search? - CS

        if (data != null && data.soweapon.type == WeaponType.Strong)
        {
            initialStrongWeapon = data.soweapon;
        }
        else
        {
            initialStrongWeapon = null; // used to reset starting weapons screen
        }
    }

    public void ApplyPermanentUpgrades(PlayerPowerupManager powerupManager) 
    {
        foreach (UpgradeTreeData upgradeTree in upgrades) 
        {
            for (int i = 0; i < upgradeTree.CurrentLevel; i++)
            {
                switch (upgradeTree.treeType)
                {
                    case UpgradeTypes.Health:
                        Debug.Log(powerupManager.playerHealth.maxHealth);
                        powerupManager.playerHealth.IncreaseMaxHealth(1);
                        powerupManager.playerHealth.Heal(1);
                        Debug.Log("Health upgrade applied! " + powerupManager.playerHealth.maxHealth);
                        break;
                    case UpgradeTypes.Speed:
                        Debug.Log(powerupManager.playerSpeed.groundController.groundSpeed);
                        powerupManager.playerSpeed.groundController.ChangeSpeed(1.03f);
                        Debug.Log("Speed upgrade applied! " + powerupManager.playerSpeed.groundController.groundSpeed);
                        break;
                    case UpgradeTypes.Bullet:
                        Debug.Log(powerupManager.playerWeapon.bulletSizeMultiplier);
                        powerupManager.playerWeapon.bulletSizeMultiplier += .2f;
                        Debug.Log("Bullet size upgrade applied! " + powerupManager.playerWeapon.bulletSizeMultiplier);
                        break;
                    case UpgradeTypes.Luck:
                        Debug.Log("Luck upgrade applied!");
                        break;
                    case UpgradeTypes.Money:
                        Debug.Log("Money upgrade applied!");
                        break;
                    default:
                        Debug.LogError("Invalid upgrade type");
                        break;
                }
            }
        }
    }

    public void Save()
    {
        SaveRunData();
        SavePermanentProgress();

        initialLightWeapon = null;
        initialStrongWeapon = null;
    }

    ProgressionSaveData GetPermanentProgress()
    {
        ProgressionSaveData data = new ProgressionSaveData();
        data.version = ProgressionSaveData.CURRENT_VERSION;

        data.permanentCurrency = permanentCurrency;
        data.upgrades = new PermanentUpgradesSaveData[upgrades.Count];
        for (int i = 0; i < upgrades.Count; i++)
        {
            data.upgrades[i] = new PermanentUpgradesSaveData();
            data.upgrades[i].level = upgrades[i].CurrentLevel;
            data.upgrades[i].type = upgrades[i].treeType;
        }

        List<BaseFeat> feats = FeatManager.instance.feats;
        data.feats = new FeatSaveData[feats.Count];

        for (int i = 0; i < feats.Count; i++)
        {
            data.feats[i] = new FeatSaveData();
            data.feats[i].featName = feats[i].name;
            data.feats[i].unlocked = feats[i].Unlocked;
        }

        data.unlockedWeapons = new Weapon[unlockedWeapons.Count];
        for (int i = 0; i < unlockedWeapons.Count; i++)
        {
            data.unlockedWeapons[i] = unlockedWeapons[i].weapon;
        }

        data.unlockedClasses = new string[unlockedClasses.Count];
        for (int i = 0; i < unlockedClasses.Count; i++)
        {
            data.unlockedClasses[i] = unlockedClasses[i];
        }

        return data;
    }

    void SaveRunData()
    {
        // Save the In-Run Progression
        RunSaveData data = new RunSaveData();
        data.version = RunSaveData.CURRENT_VERSION;

        data.currentStage = GameManager.Instance.currentSceneIndex;
        data.seed = GameManager.Instance.seed;

        data.inRunCurrency = runCurrency;
        data.SetWeapons(PlayerController.Instance.playerWeapon);

        SaveManager.Save("InRunData", data);
    }

    public void SavePermanentProgress()
    {
        // Save the Permanent Progress
        ProgressionSaveData data = GetPermanentProgress();

        ProgressionManager.instance.SaveProgressionData(ref data);

        SaveManager.Save("PermanentData", data);
    }

    public void Load()
    {
        if (!File.Exists(SaveManager.GetPath("InRunData")))
        {
            return;
        }

        RunSaveData data = SaveManager.Load<RunSaveData>("InRunData");

        if (GameManager.Instance.currentSceneIndex == data.currentStage)
        {
            GameManager.Instance.seed = data.seed;
        }

        runCurrency = data.inRunCurrency;

        PlayerController.Instance.playerWeapon.LoadWeapons(data.weapons);
    }

    void LoadPermanentData()
    {
        if (!File.Exists(SaveManager.GetPath("PermanentData")))
        {
            return;
        }

        ProgressionSaveData data = SaveManager.Load<ProgressionSaveData>("PermanentData");

        if (data.version >= 2)
        {
            ProgressionManager.instance.SetFromSaveData(data);
        }

        permanentCurrency = data.permanentCurrency;
        
        for(int i = 0; i < data.upgrades.Length; i++)
        {
            UpgradeTreeData tree = upgrades.Find(t => t.treeType == data.upgrades[i].type);
            tree.SetLevel(data.upgrades[i].level);
        }

        for (int i = 0; i < data.feats.Length; i++)
        {
            string featName = data.feats[i].featName;
            FeatManager.instance.GetFeatByName(featName)?.SetUnlockWithoutNotify(data.feats[i].unlocked);
        }

        WeaponDatabase database = Resources.Load<WeaponDatabase>("All Weapons");
        for (int i = 0; i < data.unlockedWeapons.Length; i++)
        {
            SOWeapon weapon = database.weapons.Find(weapon => weapon.weapon == data.unlockedWeapons[i]).soweapon;
            if (!unlockedWeapons.Contains(weapon))
            {
                unlockedWeapons.Add(weapon);
            }
        }

        for(int i = 0; i < data.unlockedClasses.Length; i++)
        {
            if (!unlockedClasses.Contains(data.unlockedClasses[i]))
                unlockedClasses.Add(data.unlockedClasses[i]);
        }
    }
}
