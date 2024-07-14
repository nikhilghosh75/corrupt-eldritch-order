using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PowerupDisplay : MonoBehaviour
{

    [SerializeField]
    GameObject powerupOne;
    [SerializeField]
    GameObject powerupTwo;
    [SerializeField]
    GameObject powerupThree;

    [SerializeField]
    Sprite emptySprite;

    Subscription<WeaponPowerupEvent> powerup_event;

    private void Awake()
    {
        powerup_event = EventBus.Subscribe<WeaponPowerupEvent>(OnGainWeapon);

        powerupOne.GetComponent<Image>().sprite = emptySprite;
        powerupTwo.GetComponent<Image>().sprite = emptySprite;
        powerupThree.GetComponent<Image>().sprite = emptySprite;
    }

    void OnGainWeapon(WeaponPowerupEvent e)
    {
        StartCoroutine(UpdatePowerupDisplay(e));
    }

    IEnumerator UpdatePowerupDisplay(WeaponPowerupEvent e)
    {
        yield return new WaitForSeconds(0.05f);
        powerupOne.GetComponent<Image>().sprite = (e.playerWeapon.weakWeapon ? e.playerWeapon.weakWeapon.weaponIcon : emptySprite);
        powerupTwo.GetComponent<Image>().sprite = (e.playerWeapon.strongWeapon ? e.playerWeapon.strongWeapon.weaponIcon : emptySprite);
        powerupThree.GetComponent<Image>().sprite = (e.playerWeapon.comboWeapon ? e.playerWeapon.comboWeapon.weaponIcon : emptySprite);
    }

    
}
