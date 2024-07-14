using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClassBase : MonoBehaviour
{
    public int maxExtraJumps;
    public float speed; // can be broken into air and ground if desired
    public float maxHealth;
    public float dodgeCooldown;
    public float dodgeLength;
    public float damageMultiplier;
    public float fireRate;
    public float maxMana;
    public float manaChargeRate;

    protected PlayerController playerController;
    protected SpriteRenderer sprite;
    protected Color spriteColor = new Color(1, 1, 1); // Placeholder for more distinct sprites later on

    protected void Awake()
    {
        playerController = GetComponent<PlayerController>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void OnEnable()
    {
        sprite.color = spriteColor;
        if (playerController != null)
        {
            // Ground Controller updates:
            playerController.groundController.groundSpeed *= speed;

            // Air Controller updates:
            playerController.airController.maxExtraJumps = maxExtraJumps + 1; // initial jump seems to count as an "extra" jump so this is needed
            playerController.airController.horizontalAirSpeed *= speed;
            // playerController.airController.jumpingPower *= jumpPower; // Use when jumping upgrade is added

            // Dash Controller updates: 
            playerController.dashController.dashCooldown *= dodgeCooldown;
            playerController.dashController.dashDuration *= dodgeLength;

            // Weapon updates:
            PlayerWeapon weapon = playerController.playerWeapon;
            if (weapon)
            {
                weapon.weakAttackTimer *= fireRate;
                weapon.strongAttackTimer *= fireRate;
                weapon.damageMultiplier *= damageMultiplier;

                weapon.manaMultiplier *= manaChargeRate;
            }

            // Mana updates:
            playerController.playerMana.maxMana *= maxMana;

            // Health updates:
            playerController.playerHealth.ChangeMaxHealth(maxHealth);
            playerController.playerHealth.ResetHealth();
        }
        else
        {
            Debug.Log("Error: Cannot set a class with no player controller.");
        }
    }

    public void OnDisable() // reset stats to default when swapping
    {
        // Ground Controller updates:
        playerController.groundController.groundSpeed *= 1 / speed;

        // Air Controller updates:
        playerController.airController.horizontalAirSpeed *= 1 / speed;

        // Dash Controller updates: 
        playerController.dashController.dashCooldown *= 1 / dodgeCooldown;
        playerController.dashController.dashDuration *= 1 / dodgeLength;

        // Weapon updates:
        PlayerWeapon weapon = playerController.playerWeapon;
        weapon.weakAttackTimer *= 1 / fireRate;
        weapon.strongAttackTimer *= 1 / fireRate;
        weapon.damageMultiplier *= 1 / damageMultiplier;

        // Mana updates:
        playerController.playerMana.maxMana *= 1 / maxMana;
        weapon.manaMultiplier *= 1 / manaChargeRate;

        // Health updates:
        playerController.playerHealth.ChangeMaxHealth(1 / maxHealth);
    }
}
