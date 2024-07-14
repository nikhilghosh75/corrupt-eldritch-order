using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelectScreen : MonoBehaviour
{
    bool lightWeaponSelected = false, heavyWeaponSelected = false;
    int selectedLightWeaponNum = -1, selectedHeavyWeaponNum = -1;
    private SOWeapon selectedLightWeapon, selectedHeavyWeapon;

    public List<Image> selectedIcons;
    public MenuManager mainMenuManager;
    public Image comboWeaponImage;
    public TextMeshProUGUI comboWeaponText;
    public TextMeshProUGUI comboWeaponDescription;
    private WeaponDatabase database;

   [Header("Light Display Objects")]
    public StatDisplayBar lightDamageDisplay;
    public StatDisplayBar lightFireRateDisplay; 
    public StatDisplayBar lightManaGainDisplay;
    
    [Header("Heavy Display Objects")]
    public StatDisplayBar heavyDamageDisplay;
    public StatDisplayBar heavyFireRateDisplay; 
    public StatDisplayBar heavyManaDrainDisplay;

    private int maxLightDamage = int.MinValue;
    private float maxLightFireRate = float.MinValue;
    private int maxLightManaGain = int.MinValue;

    private int maxHeavyDamage = int.MinValue;
    private float maxHeavyFireRate = float.MinValue;
    private int maxHeavyManaDrain = int.MinValue;

    private void Start()
    {
        database = Resources.Load<WeaponDatabase>("All Weapons");
        GetMaxStats();
        ResetScreen();
    }

    // Reset the starting weapons each time the screen is enabled
    private void OnEnable()
    {
        RunManager.Instance.SetStartingLightWeapon(null);
        RunManager.Instance.SetStartingStrongWeapon(null);

        ResetScreen();
    }

    private void ResetScreen()
    {
        // Reset all icons in case they were left on
        foreach (Image icon in selectedIcons)
        {
            icon.enabled = false;
        }
        comboWeaponImage.enabled = false;
        selectedHeavyWeapon = null;
        selectedLightWeapon = null;
        selectedLightWeaponNum = -1;
        selectedHeavyWeaponNum = -1;
        lightWeaponSelected = false;
        heavyWeaponSelected = false;
        UpdateStatBars(WeaponType.Weak, 0, 0, 0);
        UpdateStatBars(WeaponType.Strong, 0, 0, 0);
    }

    public void SelectLightWeapon(SOWeapon weapon)
    {
        for (int i = 0; i < 3; i++) 
        {
            selectedIcons[i].enabled = false;
        }

        lightWeaponSelected = true;
        selectedLightWeapon = weapon;
        if (weapon.weapon == Weapon.MiniMachineGun)
        {
            selectedLightWeaponNum = 0;
        }
        else if (weapon.weapon == Weapon.ManaExtractor)
        {
            selectedLightWeaponNum = 1;
        }
        else if (weapon.weapon == Weapon.Revolver)
        {
            selectedLightWeaponNum = 2;
        }
        selectedIcons[selectedLightWeaponNum].enabled = true;

        RunManager.Instance.SetStartingLightWeapon(weapon);
    }

    public void SelectHeavyWeapon(SOWeapon weapon)
    {
        for (int i = 3; i < 6; i++)
        {
            selectedIcons[i].enabled = false;
        }
        
        heavyWeaponSelected = true;
        selectedHeavyWeapon = weapon;
        if (weapon.weapon == Weapon.Sniper)
        {
            selectedHeavyWeaponNum = 3;
        }
        else if (weapon.weapon == Weapon.Flamethrower)
        {
            selectedHeavyWeaponNum = 4;
        }
        else if (weapon.weapon == Weapon.Explosive)
        {
            selectedHeavyWeaponNum = 5;
        }
        selectedIcons[selectedHeavyWeaponNum].enabled = true;
        
        RunManager.Instance.SetStartingStrongWeapon(weapon);
    }

    public void ConfirmButtonClick()
    {
        if (lightWeaponSelected && heavyWeaponSelected)
            mainMenuManager.TransitionToCanvas("Start Run Menu Canvas");
    }

    public void ChangeStats(SOWeapon weapon) 
    {
        if (weapon.type == WeaponType.Weak)
        {
            float damagePercent = (float)weapon.damage / maxLightDamage;
            float fireRatePercent = (1 / weapon.fireCooldown) / maxLightFireRate;
            float manaGainPercent = (float)weapon.manaRecovered / maxLightManaGain;

            UpdateStatBars(WeaponType.Weak, damagePercent, fireRatePercent, manaGainPercent);
        }
        else if (weapon.type == WeaponType.Strong) 
        {
            float damagePercent = (float)weapon.damage / maxHeavyDamage;
            float fireRatePercent = (1 / weapon.fireCooldown) / maxHeavyFireRate;
            float manaDrainPercent = (float)weapon.manaConsumed / maxHeavyManaDrain;

            UpdateStatBars(WeaponType.Strong, damagePercent, fireRatePercent, manaDrainPercent);
        }

        if (heavyWeaponSelected && lightWeaponSelected)
        {
            if (comboWeaponImage.enabled == false)
            {
                comboWeaponImage.enabled = true;
                comboWeaponText.enabled = true;
            }

            SOComboWeapon comboWeapon = database.GetComboWeapon(selectedLightWeapon, selectedHeavyWeapon);
            Debug.Log(comboWeapon.weaponType.ToString());
            comboWeaponImage.sprite = comboWeapon.weaponSprite;
            comboWeaponText.text = Regex.Replace(comboWeapon.weaponType.ToString(), "(\\B[A-Z])", " $1");
            comboWeaponDescription.text = comboWeapon.descriptionText;
        }
    }

    private void UpdateStatBars(WeaponType weaponType, float damagePercent, float fireRatePercent, float manaPercent) 
    {
        if (weaponType == WeaponType.Weak)
        {
            lightDamageDisplay.UpdateStatBar(damagePercent);
            lightFireRateDisplay.UpdateStatBar(fireRatePercent);
            lightManaGainDisplay.UpdateStatBar(manaPercent);
        }
        else if (weaponType == WeaponType.Strong) 
        {
            heavyDamageDisplay.UpdateStatBar(damagePercent);
            heavyFireRateDisplay.UpdateStatBar(fireRatePercent);
            heavyManaDrainDisplay.UpdateStatBar(manaPercent);
        }
    }

    private void GetMaxStats()
    {
        foreach (WeaponDatabase.Data weaponData in database.weapons)
        {
            SOWeapon weapon = weaponData.soweapon;
            if (weapon.type == WeaponType.Weak)
            {
                maxLightDamage = Mathf.Max(maxLightDamage, weapon.damage);
                maxLightFireRate = Mathf.Max(maxLightFireRate, 1 / weapon.fireCooldown);
                maxLightManaGain = Mathf.Max(maxLightManaGain, weapon.manaRecovered);
            }
            else if (weapon.type == WeaponType.Strong) {
                maxHeavyDamage = Mathf.Max(maxHeavyDamage, weapon.damage);
                maxHeavyFireRate = Mathf.Max(maxHeavyFireRate, 1 / weapon.fireCooldown);
                maxHeavyManaDrain = Mathf.Max(maxHeavyManaDrain, weapon.manaConsumed);
            }
        }
    }
}
