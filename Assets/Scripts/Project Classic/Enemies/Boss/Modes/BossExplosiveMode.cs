using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExplosiveMode : BossMode
{
    // Explosion prefab for the boss
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float explosionRadius = 2f;

    public override int ModeID { get { return 0; } }

    public override void OnBehaviorStart()
    {
        // Get all attacks on the boss
        EnemyFireProjectileAction[] attackComponents = GetComponents<EnemyFireProjectileAction>();

        // Make all projectiles explosive
        foreach (EnemyFireProjectileAction action in attackComponents)
        {
            if (action.projectileData)
            {
                ProjectileData newProjectileData = Instantiate(action.projectileData);

                newProjectileData.isExplosive = true;
                newProjectileData.explosionPrefabs.Add(explosionPrefab);
                newProjectileData.explosionRadii.Add(explosionRadius);

                action.projectileData = newProjectileData;
            }              
        }
    }
}
