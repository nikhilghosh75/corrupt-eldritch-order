using System;

[Serializable]
public class WeaponSaveData
{
    public Weapon weapon;
    public string[] modNames;
}

[Serializable]
public class RunSaveData
{
    public const int CURRENT_VERSION = 1;

    /*
     * This version should be used for ensuring a struct has all the necessary data elements
     */
    public int version = 1;

    public int seed = 0;
    public int currentStage = 0;

    public int inRunCurrency = 0;

    public WeaponSaveData[] weapons;

    public void SetWeapons(PlayerWeapon weapon)
    {
        weapons = new WeaponSaveData[3];

        weapons[0] = new WeaponSaveData();
        weapons[0].weapon = weapon.weakWeapon.weapon;
        weapons[0].modNames = new string[weapon.weakWeaponMods.Count];
        for (int i = 0; i < weapon.weakWeaponMods.Count; i++)
        {
            weapons[0].modNames[i] = weapon.weakWeaponMods[i].displayName;
        }

        weapons[1] = new WeaponSaveData();
        weapons[1].weapon = weapon.strongWeapon.weapon;
        weapons[1].modNames = new string[weapon.strongWeaponMods.Count];
        for (int i = 0; i < weapon.strongWeaponMods.Count; i++)
        {
            weapons[1].modNames[i] = weapon.strongWeaponMods[i].displayName;
        }

        weapons[2] = new WeaponSaveData();
        weapons[2].weapon = weapon.comboWeapon.weaponType;
        weapons[2].modNames = new string[0];
    }
}

[Serializable]
public class PermanentUpgradesSaveData
{
    public UpgradeTypes type;
    public int level;
}

[Serializable]
public class FeatSaveData
{
    public string featName;
    public bool unlocked;
}

[Serializable]
public class DamagedByStatSaveData
{
    public string damagedBy;
    public int numTimesDamagedBy;

    public DamagedByStatSaveData(string _damagedBy, int _numTimes)
    {
        damagedBy = _damagedBy;
        numTimesDamagedBy = _numTimes;
    }
}

[Serializable]
public class ProgressionSaveData
{
    public const int CURRENT_VERSION = 3;

    /*
     * This version should be used for ensuring a struct has all the necessary data elements
     */
    public int version = 1;

    public int permanentCurrency;

    public PermanentUpgradesSaveData[] upgrades;
    public FeatSaveData[] feats;

    public Weapon[] unlockedWeapons;
    public string[] unlockedClasses;

    // Version 2 - Added functionality to save items from the progression manager
    public int jumpCount = 0;
    public int enemiesKilledCount = 0;
    public int bossDefeatedCount = 0;
    public int permanentCurrencyAcquired = 0;

    // Version 3 - Added functionality to save the "damaged by" and individual enemies killed stats
    public int soldiersKilledCount = 0;
    public int dronesKilledCount = 0;
    public int infestationKilledCount = 0;
    public int warlockKilledCount = 0;
    public int debufferKilledCount = 0;
    public DamagedByStatSaveData[] damagedBy;
}
