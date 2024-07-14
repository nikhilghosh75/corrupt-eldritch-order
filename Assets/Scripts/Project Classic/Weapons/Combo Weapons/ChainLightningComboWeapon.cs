using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon", menuName = "Project Classic/Combo Weapons/Chain Lightning")]
public class ChainLightningComboWeapon : SOComboWeapon
{
    public float damageMultiplier;
    public float bulletSpeedMultiplier;

    public int numberOfChains;
    public float damageChainMultiplier;
    public float speedChainMultiplier;
    public LayerMask enemyLayerMask;
    public LayerMask levelLayerMask;

    float strongAttackFireTime;
    float weakAttackFireTime;

    public override void EndComboWeapon(PlayerWeapon playerWeapon)
    {
        playerWeapon.damageMultiplier /= damageMultiplier;
        playerWeapon.bulletSpeedMultiplier /= bulletSpeedMultiplier;
    }

    public override void StartComboWeapon(PlayerWeapon playerWeapon)
    {
        playerWeapon.damageMultiplier *= damageMultiplier;
        playerWeapon.bulletSpeedMultiplier *= bulletSpeedMultiplier;
    }

    public override void OnProjectileSpawn(Projectile projectile)
    {
        base.OnProjectileSpawn(projectile);

        ChainOnCollide chain = projectile.AddComponent<ChainOnCollide>();
        chain.numberOfChains = numberOfChains;
        chain.speedMultiplier = speedChainMultiplier;
        chain.damageMultiplier = damageChainMultiplier;
        chain.levelLayerMask = levelLayerMask;
        chain.enemyLayerMask = enemyLayerMask;

        projectile.GetComponent<Projectile>().piercesLeft = numberOfChains;
    }

    public override void OnFire(PlayerWeapon playerWeapon, SOWeapon firedWeapon)
    {
        base.OnFire(playerWeapon, firedWeapon);

        if (firedWeapon.type == WeaponType.Weak)
        {
            if (Time.time >= weakAttackFireTime)
            {
                playerWeapon.FireWeapon(firedWeapon);
                weakAttackFireTime = Time.time + firedWeapon.fireCooldown;
            }
        }
        else if (firedWeapon.type == WeaponType.Strong)
        {
            if (Time.time >= strongAttackFireTime)
            {
                playerWeapon.FireWeapon(firedWeapon);
                strongAttackFireTime = Time.time + firedWeapon.fireCooldown;
            }
        }
    }
}
