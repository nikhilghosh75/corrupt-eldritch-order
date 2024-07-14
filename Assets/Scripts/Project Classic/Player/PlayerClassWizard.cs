using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClassWizard : PlayerClassBase
{
    public int numExtraJumps = 1;
    public float speedMod = 1f;
    public float healthMod = 1f;
    public float dodgeCDMod = 1f;
    public float dodgeLengthMod = 1f;
    public float damageMod = .7f;
    public float fireRateMod = 1f;
    public float maxManaMod = 1f;
    public float manaChargeMod = 1.5f;

    private void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        maxExtraJumps = numExtraJumps;
        speed = speedMod; // can be broken into air and ground if desired
        maxHealth = healthMod;
        dodgeCooldown = dodgeCDMod;
        dodgeLength = dodgeLengthMod;
        damageMultiplier = damageMod;
        fireRate = fireRateMod;
        maxMana = maxManaMod;
        manaChargeRate = manaChargeMod;
        spriteColor = new Color(0, 1, 1);
        base.OnEnable(); // handles adjusting these values for the player
    }

    public void OnDisable()
    {
        spriteColor = new Color(1, 1, 1);
        base.OnDisable();
    }
}
