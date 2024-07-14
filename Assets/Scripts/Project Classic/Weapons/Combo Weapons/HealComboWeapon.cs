using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Project Classic/Combo Weapons/Heal")]
public class HealComboWeapon : SOComboWeapon
{
    public float timeBetweenHeals;

    Coroutine coroutine;

    public override void EndComboWeapon(PlayerWeapon playerWeapon)
    {
        playerWeapon.StopCoroutine(coroutine);
    }

    public override void StartComboWeapon(PlayerWeapon playerWeapon)
    {
        PlayerHealth playerHealth = playerWeapon.playerController.playerHealth;
        coroutine = playerWeapon.StartCoroutine(PerformHeals(playerHealth));
    }

    IEnumerator PerformHeals(PlayerHealth playerHealth)
    {
        while(true)
        {
            playerHealth.Heal(1);
            yield return timeBetweenHeals;
        }
    }
}
