using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Database", menuName = "Weapon Database")]
public class WeaponDatabase : ScriptableObject
{
    [System.Serializable]
    public class Data
    {
        public SOWeapon soweapon;
        public string name;
        public WeaponType type;
        public Weapon weapon;
        public GameObject droppedPowerupPrefab;
    }

    [System.Serializable]
    public class ComboData
    {
        public SOWeapon weakWeapon;
        public SOWeapon strongWeapon;
        public SOComboWeapon comboWeapon;
    }

    public List<Data> weapons;
    public List<ComboData> comboData;

    private Dictionary<Tuple<SOWeapon, SOWeapon>, SOComboWeapon> comboWeapons;

    public void Init()
    {
        comboWeapons = new Dictionary<Tuple<SOWeapon, SOWeapon>, SOComboWeapon>();

        foreach (var combo in comboData)
        {
            comboWeapons.Add(Tuple.Create(combo.weakWeapon, combo.strongWeapon), combo.comboWeapon);
        }
    }

    public SOComboWeapon GetComboWeapon(SOWeapon weakWeapon, SOWeapon strongWeapon)
    {
        if (comboWeapons == null)
            Init();

        if (weakWeapon == null || strongWeapon == null) return null;

        Tuple<SOWeapon, SOWeapon> key = Tuple.Create(weakWeapon, strongWeapon);

        if (comboWeapons.ContainsKey(key))
        {
            return comboWeapons[key];
        }
        else
        {
            return null;
        }
    }

    public List<SOWeapon> GetAllDefaultUnlockedWeapons()
    {
        List<SOWeapon> defaultWeapons = new List<SOWeapon>();

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].soweapon.unlockedByDefault)
            {
                defaultWeapons.Add(weapons[i].soweapon);
            }
        }

        return defaultWeapons;
    }
}

public enum WeaponType
{
    Weak,
    Strong,
    Combo
}

public enum Weapon
{
    Basic,
    MiniMachineGun,
    Revolver,
    ManaExtractor,
    Explosive,
    Sniper,
    Flamethrower,
    TripleExplosiveMachineGun,
    ImpactGun,
    ManaBurst,
    BouncingBlades,
    SuperSniper,
    ChainLightning,
    LaserShotgun,
    RageAura,
    Heal
}
