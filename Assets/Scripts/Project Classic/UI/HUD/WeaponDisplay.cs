using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplay : MonoBehaviour
{
    public Image lightWeaponImage;
    public Image strongWeaponImage;
    public Image comboWeaponImage;

    [SerializeField]
    public WeaponDatabase weaponDatabase;

    void Start()
    {
        SOWeapon lightWeapon;
        SOWeapon strongWeapon;

        EventBus.Subscribe<WeaponPowerupEvent>(OnGainWeaponPowerup);

        GameObject playerWeapon = GameObject.FindGameObjectWithTag("PlayerWeapon");
        PlayerWeapon weaponScript = playerWeapon.GetComponent<PlayerWeapon>();

        lightWeapon = weaponScript.weakWeapon;
        strongWeapon = weaponScript.strongWeapon;

        if (lightWeapon != null && strongWeapon != null)
        {
            lightWeaponImage.sprite = lightWeapon.weaponSprite;
            strongWeaponImage.sprite = strongWeapon.weaponSprite;
            comboWeaponImage.sprite = weaponDatabase.GetComboWeapon(lightWeapon, strongWeapon).weaponSprite;
        }
        else 
        {
            Debug.LogError("Light or strong weapon is null!");
        }


        ResizeWeaponImagesToFitSprites();
    }

    private void OnGainWeaponPowerup(WeaponPowerupEvent e)
    {
        if (e.weaponPowerup.weapon.type == WeaponType.Weak)
        {
            lightWeaponImage.sprite = e.weaponPowerup.weapon.weaponSprite;
            
        }
        else if (e.weaponPowerup.weapon.type == WeaponType.Strong) 
        {
            strongWeaponImage.sprite = e.weaponPowerup.weapon.weaponSprite;
        }

        comboWeaponImage.sprite = e.playerWeapon.comboWeapon.weaponSprite;

        ResizeWeaponImagesToFitSprites();
    }

    private void ResizeWeaponImagesToFitSprites()
    {
        if (lightWeaponImage.sprite != null)
        {
            RectTransform rectTransform = lightWeaponImage.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(lightWeaponImage.sprite.rect.width, lightWeaponImage.sprite.rect.height);
        }
        if (strongWeaponImage.sprite != null)
        {
            RectTransform rectTransform = strongWeaponImage.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(strongWeaponImage.sprite.rect.width, strongWeaponImage.sprite.rect.height);
        }
        if (comboWeaponImage.sprite != null)
        {
            RectTransform rectTransform = comboWeaponImage.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(comboWeaponImage.sprite.rect.width, comboWeaponImage.sprite.rect.height);
        }
    }
}
