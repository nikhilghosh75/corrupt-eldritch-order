using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerupManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public PlayerWeapon playerWeapon;
    public PlayerController playerSpeed;
    public PlayerInvincibilityController invincibilityController;

    public float pickupRange;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerWeapon = GetComponentInChildren<PlayerWeapon>();
        playerSpeed = GetComponent<PlayerController>();
        invincibilityController = GetComponent<PlayerInvincibilityController>();
    }
}

class WeaponPowerupEvent
{
    public SOWeaponPowerup weaponPowerup;
    public PlayerWeapon playerWeapon;

    public WeaponPowerupEvent(SOWeaponPowerup weaponPowerup, PlayerWeapon playerWeapon)
    {
        this.weaponPowerup = weaponPowerup;
        this.playerWeapon = playerWeapon;
    }

}

class PowerupEvent
{
    public SOPowerup powerup;
    public PowerupEvent(SOPowerup powerup)
    {
        this.powerup = powerup;
    }
}