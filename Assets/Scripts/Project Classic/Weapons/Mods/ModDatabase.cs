using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mod Database", menuName = "Mod Database")]
public class ModDatabase : ScriptableObject
{
    public List<WeaponMod> mods;

    public List<WeaponMod> common;
    public List<WeaponMod> uncommon;
    public List<WeaponMod> rare;
    public List<WeaponMod> epic;
    public List<WeaponMod> legendary;
}
