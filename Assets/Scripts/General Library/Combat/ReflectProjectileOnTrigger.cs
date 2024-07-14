using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Combat;

/*
 * Gives weapon collider ability to reflect enemy projectiles in Project Drift. 
 * Enable/disable this component to add/remove behavior.
 * Kevin Han '23
 */
namespace WSoft.Combat
{
    public class ReflectProjectileOnTrigger : MonoBehaviour
    {
        [Tooltip("Amount of damage reflected project deals to enemies")]
        [SerializeField] int reflectedProjectileDamage = 1;
        [Tooltip("If true, the amount of damage projectile would do to player is what it would deal to enemeis. Else use reflectedProjectileDamage.")]
        [SerializeField] bool useOriginalProjectileDamage = false;
        [Tooltip("Use to increase/decrease speed of reflected projectile")]
        [SerializeField] float reflectedSpeedMultiplier = 0.5f;
        [Tooltip("The new layermask that the reflected projectile can damage")]
        [SerializeField] LayerMask newLayerMask;
        [Tooltip("The game object to be spawned on trigger. Set to null for nothing to spawn")]
        [SerializeField] GameObject reflectParticle;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!enabled)
                return;

            ProjectileMovement2D projMove = other.GetComponent<ProjectileMovement2D>();
            if (projMove != null)
            {
                if (reflectParticle)
                    Instantiate(reflectParticle, other.transform.position, Quaternion.identity);

                // Change projectile trajectory
                projMove.direction *= -1;
                projMove.speed *= reflectedSpeedMultiplier;

                // Change projectile to damage enemies (instead of player)
                DamageOnCollision2D projDmg = other.gameObject.GetComponent<DamageOnCollision2D>();
                projDmg.damageLayers = newLayerMask;
                if (!useOriginalProjectileDamage) projDmg.damage = reflectedProjectileDamage;

                // Change projectile to self destruct on contact with enemies (instead of player)
                DestroyOnCollision2D projSelfDestroy = other.gameObject.GetComponent<DestroyOnCollision2D>();
                projSelfDestroy.layerMaskToDestroy = newLayerMask;

                // Change layer
                int playerAttackLayer = gameObject.layer;
                other.gameObject.layer = playerAttackLayer;
            }
        }
    }
}