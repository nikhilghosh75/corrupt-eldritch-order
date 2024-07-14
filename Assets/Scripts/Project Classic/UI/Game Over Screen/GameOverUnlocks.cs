using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUnlocks : MonoBehaviour
{
    static readonly List<string> AllClasses = new List<string>() { "Default", "Tank", "Wizard", "Ninja" };

    [Serializable]
    public class UnlockDisplay
    {
        public Image image;
        public TMP_Text text;
    }

    public List<UnlockDisplay> unlockDisplays;

    public Sprite defaultIcon;
    public Sprite tankIcon;
    public Sprite wizardIcon;
    public Sprite ninjaIcon;

    public float timeBetweenUnlocks;

    public IEnumerator ShowUnlocks()
    {
        ResetUnlockDisplays();
        int index = 0;

        List<string> unlockedClasses = GetUnlockedClassesThisRun();
        for (int i = 0; i < unlockedClasses.Count; i++)
        {
            unlockDisplays[index].image.sprite = GetClassIcon(unlockedClasses[i]);
            unlockDisplays[index].text.text = unlockedClasses[i];
            index++;
        }

        List<SOWeapon> unlockedWeapons = GetUnlockedWeaponsThisRun();
        for (int i = 0; i < unlockedWeapons.Count; i++)
        {
            if (index >= unlockDisplays.Count)
            {
                break;
            }

            unlockDisplays[index].image.sprite = unlockedWeapons[i].weaponSprite;
            unlockDisplays[index].text.text = unlockedWeapons[i].name;
            index++;
        }

        for (int i = 0; i < index; i++)
        {
            unlockDisplays[i].text.gameObject.SetActive(true);
            unlockDisplays[i].image.gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(timeBetweenUnlocks);
        }
    }

    List<string> GetUnlockedClassesThisRun()
    {
        ProgressionSaveData startOfRunSaveData = RunManager.Instance.saveDataAtStartOfRun;
        string[] startOfRunClasses = startOfRunSaveData.unlockedClasses;
        List<string> unlockedClasses = RunManager.Instance.unlockedClasses;

        List<string> classesUnlockedThisRun = new List<string>();
        for (int i = 0; i < AllClasses.Count; i++)
        {
            if (unlockedClasses.Contains(AllClasses[i]) && !startOfRunClasses.Contains(AllClasses[i]))
            {
                classesUnlockedThisRun.Add(AllClasses[i]);
            }
        }

        return classesUnlockedThisRun;
    }

    List<SOWeapon> GetUnlockedWeaponsThisRun()
    {
        ProgressionSaveData startOfRunSaveData = RunManager.Instance.saveDataAtStartOfRun;
        Weapon[] startOnRunWeapons = startOfRunSaveData.unlockedWeapons;
        List<SOWeapon> unlockedWeapons = RunManager.Instance.unlockedWeapons;

        Debug.Log(startOnRunWeapons.Length);
        Debug.Log(unlockedWeapons.Count);

        List<SOWeapon> weaponsUnlockedThisRun = new List<SOWeapon>();
        for (int i = 0; i < unlockedWeapons.Count; i++)
        {
            if (!startOnRunWeapons.Contains(unlockedWeapons[i].weapon))
            {
                weaponsUnlockedThisRun.Add(unlockedWeapons[i]);
            }
        }
        return weaponsUnlockedThisRun;
    }

    Sprite GetClassIcon(string _class)
    {
        switch(_class)
        {
            case "Default": return defaultIcon;
            case "Ninja": return ninjaIcon;
            case "Wizard": return wizardIcon;
            case "Tank": return tankIcon;
        }

        return null;
    }

    public void ResetUnlockDisplays()
    {
        foreach (UnlockDisplay display in unlockDisplays)
        {
            display.image.gameObject.SetActive(false);
            display.text.gameObject.SetActive(false);
            display.image.sprite = null;
            display.text.text = "";
        }
    }
}
