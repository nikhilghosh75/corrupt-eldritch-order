using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pickup", menuName = "Project Classic/Powerup")]
public class SOWeaponPowerup : SOPowerup
{
    public Sprite powerupSprite;
    public SOWeapon weapon;
    public Rarity rarity;
    public List<WeaponMod> weaponMods;

    public SOWeaponPowerup(Sprite powerupSprite, SOWeapon weapon)
    {
        this.powerupSprite = powerupSprite;
        this.weapon = weapon;
    }

    public Rarity GenerateRarity(System.Random random)
    {
        weaponMods.Clear();
        int randomValue = random.Next(100);

        int luck = (RunManager.Instance != null) ? RunManager.Instance.luck : 0;

        int[] rarityPercentages = { 45 * (1 - luck), 30 * (1 - luck/2), 15, 7 * (1 + luck), 3 * (1 + luck/2) };

        int cumulativePercentage = 0;
        for (int i = 0; i < rarityPercentages.Length; i++)
        {
            if (randomValue >= cumulativePercentage && randomValue < cumulativePercentage + rarityPercentages[i])
            {
                rarity = (Rarity)i;
            }
            cumulativePercentage += rarityPercentages[i];
        }

        ModDatabase database = Resources.Load<ModDatabase>("All Mods");

        switch (rarity)
        {
            case Rarity.Common:
                weaponMods.Add(database.common[random.Next(database.common.Count)]);
                break;
            case Rarity.Uncommon:
                weaponMods.Add(database.common[random.Next(database.common.Count)]);
                weaponMods.Add(database.uncommon[random.Next(database.uncommon.Count)]);
                break;
            case Rarity.Rare:
                weaponMods.Add(database.common[random.Next(database.common.Count)]);
                weaponMods.Add(database.uncommon[random.Next(database.uncommon.Count)]);
                weaponMods.Add(database.rare[random.Next(database.rare.Count)]);
                break;
            case Rarity.Epic:
                weaponMods.Add(database.common[random.Next(database.common.Count)]);
                weaponMods.Add(database.uncommon[random.Next(database.uncommon.Count)]);
                weaponMods.Add(database.rare[random.Next(database.rare.Count)]);
                weaponMods.Add(database.epic[random.Next(database.epic.Count)]);
                break;
            case Rarity.Legendary:
                weaponMods.Add(database.common[random.Next(database.common.Count)]);
                weaponMods.Add(database.uncommon[random.Next(database.uncommon.Count)]);
                weaponMods.Add(database.rare[random.Next(database.rare.Count)]);
                weaponMods.Add(database.epic[random.Next(database.epic.Count)]);
                weaponMods.Add(database.legendary[random.Next(database.legendary.Count)]);
                break;
        }

        return rarity;
    }

    public override bool ApplyPowerup(PlayerPowerupManager target) 
    {
        target.playerWeapon.UpdateWeaponSprite(weapon);
        switch(weapon.type)
        {
            case WeaponType.Weak:
                if (target.playerWeapon.weakWeapon != null)
                {
                    WeaponDatabase database = target.playerWeapon.weaponDatabase;
                    WeaponDatabase.Data data = database.weapons.Find(x => x.weapon == target.playerWeapon.weakWeapon.weapon);
                    Instantiate(data.droppedPowerupPrefab, target.transform.position, target.transform.rotation);
                    target.playerWeapon.SetWeapon(weapon);
                    target.playerWeapon.weakWeaponMods = weaponMods;
                }
                else
                {
                    target.playerWeapon.SetWeapon(weapon);
                    target.playerWeapon.weakWeaponMods = weaponMods;
                }
                break;

            case WeaponType.Strong:
                if (target.playerWeapon.strongWeapon != null)
                {
                    WeaponDatabase database = target.playerWeapon.weaponDatabase;
                    WeaponDatabase.Data data = database.weapons.Find(x => x.weapon == target.playerWeapon.strongWeapon.weapon);
                    Instantiate(data.droppedPowerupPrefab, target.transform.position, target.transform.rotation);
                    target.playerWeapon.SetWeapon(weapon);
                    target.playerWeapon.strongWeaponMods = weaponMods;
                }
                else
                {
                    target.playerWeapon.SetWeapon(weapon);
                    target.playerWeapon.strongWeaponMods = weaponMods;
                }
                break;
        }

        EventBus.Publish<WeaponPowerupEvent>(new WeaponPowerupEvent(this, target.playerWeapon));
        return true;
    }


}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}