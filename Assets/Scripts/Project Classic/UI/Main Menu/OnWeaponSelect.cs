using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using WSoft.Audio;

public class OnWeaponSelect : MonoBehaviour
{
    private Button myButton;

    public WeaponSelectScreen weaponScreenScript;
    public SOWeapon weapon;
    public Image lockIcon;
    public FeatDisplay lockFeatDisplay;

    private ProjectClassic controls;

    public bool selectedByDefault;

    public AudioEvent locked;

    private void Awake()
    {
        myButton = GetComponent<Button>();
        if (!RunManager.Instance.unlockedWeapons.Contains(weapon)) 
        {
            Image image = gameObject.GetComponent<Image>();
            image.color = Color.red;
            myButton.interactable = false;
            lockFeatDisplay.gameObject.SetActive(true);
        }
        else
        {
            lockIcon.enabled = false;
            lockFeatDisplay.gameObject.SetActive(false);
        }
        controls = new ProjectClassic();
        controls.UI.Navigate.performed += context => OnNavigate(context);

        myButton.onClick.AddListener(SelectWeapon);

        if (selectedByDefault)
            SelectWeapon();

    }

    private void OnEnable()
    {
        if (selectedByDefault)
            SelectWeapon();
    }

    private void OnNavigate(InputAction.CallbackContext context) 
    {
        Debug.Log("Navigated!");
        SelectWeapon();
    }

    void SelectWeapon()
    {
        if (!RunManager.Instance.unlockedWeapons.Contains(weapon))
        {
            locked.PlayAudio(gameObject);
            return;
        }

        if (weapon.type == WeaponType.Weak)
        {
            weaponScreenScript.SelectLightWeapon(weapon);
        }
        else if (weapon.type == WeaponType.Strong)
        {
            weaponScreenScript.SelectHeavyWeapon(weapon);
        }
        weaponScreenScript.ChangeStats(weapon);
    }
}
