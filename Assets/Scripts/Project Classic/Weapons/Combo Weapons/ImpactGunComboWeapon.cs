using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Project Classic/Combo Weapons/Impact")]
public class ImpactGunComboWeapon : SOComboWeapon
{
    public float damageMultiplier;
    public float explosionRadius;
    public GameObject explosionPrefab;

    float strongAttackFireTime;
    float weakAttackFireTime;

    public override void StartComboWeapon(PlayerWeapon playerWeapon)
    {
        playerWeapon.damageMultiplier *= damageMultiplier;
    }

    public override void EndComboWeapon(PlayerWeapon playerWeapon)
    {
        playerWeapon.damageMultiplier /= damageMultiplier;

        /*
        List<Projectile> weakProjectiles = ProjectilePool.Instance.projectilePools[typeof(revolverProjectile)];
        foreach (Projectile projectile in weakProjectiles)
        {
            ExplosiveProjectile explosive = projectile.GetComponent<ExplosiveProjectile>();
            if (explosive)
            {
                Destroy(explosive);
            }
        }
        */
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

    public override void OnProjectileSpawn(Projectile projectile)
    {
        base.OnProjectileSpawn(projectile);

        ExplosiveProjectile explosiveProjectile = projectile.AddComponent<ExplosiveProjectile>();
        explosiveProjectile.damage = 50;
        explosiveProjectile.explosionRadius = explosionRadius;
        explosiveProjectile.explosionPrefab = explosionPrefab;
    }
}
