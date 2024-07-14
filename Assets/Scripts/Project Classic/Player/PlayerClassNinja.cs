using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClassNinja : PlayerClassBase
{
    public int numExtraJumps = 2;
    public float speedMod = 1.5f;
    public float healthMod = .6f;
    public float dodgeCDMod = 1f;
    public float dodgeLengthMod = 1.5f;
    public float damageMod = .75f;
    public float fireRateMod = 1.25f;
    public float maxManaMod = 1f;
    public float manaChargeMod = 1f;

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
        playerController.dashController.airDodge = true;
        spriteColor = new Color(0, 0, 1);
        base.OnEnable(); // handles adjusting these values for the player
    }

    public void OnDisable()
    {
        playerController.dashController.airDodge = false;
        spriteColor = new Color(1, 1, 1);
        base.OnDisable();
    }
}
