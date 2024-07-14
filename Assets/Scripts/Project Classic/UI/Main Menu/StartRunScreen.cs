using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Unity.VisualScripting;


public class StartRunScreen : MonoBehaviour
{
    public TextMeshProUGUI selectedClassText;
    public TextMeshProUGUI lightWeaponText, strongWeaponText, comboWeaponText;
    public GameObject lightWeaponIcon, strongWeaponIcon, comboWeaponIcon;

    public Image classIcon;
    public Sprite defaultIcon, ninjaIcon, wizardIcon, tankIcon;

    [SerializeField]
    Sprite emptySprite;

    private void OnEnable()
    {
        WeaponDatabase database = Resources.Load<WeaponDatabase>("All Weapons");

        SOWeapon lightWeapon = RunManager.Instance.initialLightWeapon;
        SOWeapon strongWeapon = RunManager.Instance.initialStrongWeapon;
        SOComboWeapon comboWeapon = database.GetComboWeapon(lightWeapon, strongWeapon);
        string className = RunManager.Instance.selectedClassName;
        selectedClassText.text = className;

        if (className == "Default")
        {
            classIcon.sprite = defaultIcon;
        }
        else if (className == "Ninja")
        {
            classIcon.sprite = ninjaIcon;
        }
        else if (className == "Wizard")
        {
            classIcon.sprite = wizardIcon;
        }
        else if (className == "Tank")
        {
            classIcon.sprite = tankIcon;
        }

        lightWeaponText.text = Regex.Replace(lightWeapon.name, "(?<!^)([A-Z])", " $1");
        strongWeaponText.text = Regex.Replace(strongWeapon.name, "(?<!^)([A-Z])", " $1");
        comboWeaponText.text = Regex.Replace(comboWeapon.name, "(?<!^)([A-Z])", " $1");
        lightWeaponIcon.GetComponent<Image>().sprite = (lightWeapon.weaponSprite ? lightWeapon.weaponSprite : emptySprite);
        strongWeaponIcon.GetComponent<Image>().sprite = (strongWeapon.weaponSprite ? strongWeapon.weaponSprite : emptySprite);
        comboWeaponIcon.GetComponent<Image>().sprite = (comboWeapon.weaponSprite ? comboWeapon.weaponSprite : emptySprite);
    }
}
