using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPerkDisplay : MonoBehaviour
{
    [SerializeField]
    private List<Image> lightPerkImages, strongPerkImages;

    [SerializeField]
    private bool setupModIconEffects;

    private PlayerWeapon weaponScript;


    void Start()
    {
        weaponScript = PlayerController.Instance.playerWeapon;
        BlankOutPerkImages();
    }

    void Update()
    {
        UpdatePerkUI();
    }

    void BlankOutPerkImages()
    {
        for (int i = 0; i < lightPerkImages.Count; i++)
        {
            lightPerkImages[i].sprite = null;
        }
        for (int i = 0; i < strongPerkImages.Count; i++)
        {
            strongPerkImages[i].sprite = null;
        }
    }

    void UpdatePerkUI()
    {
        // Update light weapon mods
        for (int i = 0; i < lightPerkImages.Count; i++)
        {
            if (weaponScript.weakWeaponMods.Count > i)
            {
                lightPerkImages[i].sprite = weaponScript.weakWeaponMods[i].icon;
                lightPerkImages[i].gameObject.SetActive(true);
                
                if (setupModIconEffects)
                {
                    lightPerkImages[i].gameObject.GetComponent<ModIcon>().currentMod = weaponScript.weakWeaponMods[i];
                }
            }
            else
            {
                lightPerkImages[i].gameObject.SetActive(false);

                if (setupModIconEffects)
                {
                    lightPerkImages[i].gameObject.GetComponent<ModIcon>().currentMod = null;
                }
            }
        }

        // Update strong weapon mods
        for (int i = 0; i < strongPerkImages.Count; i++)
        {
            if (weaponScript.strongWeaponMods.Count > i)
            {
                strongPerkImages[i].sprite = weaponScript.strongWeaponMods[i].icon;
                strongPerkImages[i].gameObject.SetActive(true);

                if (setupModIconEffects)
                {
                    strongPerkImages[i].gameObject.GetComponent<ModIcon>().currentMod = weaponScript.strongWeaponMods[i];
                }
            }
            else
            {
                strongPerkImages[i].gameObject.SetActive(false);

                if (setupModIconEffects)
                {
                    strongPerkImages[i].gameObject.GetComponent<ModIcon>().currentMod = null;
                }
            }
        }
    }
}
