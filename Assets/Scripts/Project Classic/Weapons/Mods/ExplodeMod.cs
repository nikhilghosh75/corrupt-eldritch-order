using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Explode Mod", menuName = "Project Classic/Mods/Explode Mod")]
public class ExplodeMod : WeaponMod
{
    public float radius;
    public GameObject prefab;

    public override void Apply(Projectile projectile)
    {
        base.Apply(projectile);

        ExplosiveProjectile explosiveProjectile = projectile.AddComponent<ExplosiveProjectile>();
        explosiveProjectile.damage = 0;
        explosiveProjectile.explosionRadius = radius;
        explosiveProjectile.explosionPrefab = prefab;

        EventBus.Publish(new ModActivatedEvent(this));
    }
}
