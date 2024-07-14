/* 
 * Applies damage through OnCollisionEnter2D, uses a layermask to filter
 * @ Max Perraut '20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Combat
{
    [RequireComponent(typeof(Collider))]
    public class DamageOnCollision : MonoBehaviour
    {
        [Tooltip("The amount of damage that should be inflicted")]
        public int damage;

        [Tooltip("Only damages objects on these layers.")]
        public LayerMask damageLayers;

        /// <summary>
        /// On a collision, find a health component on the specified object and damage.
        /// </summary>
        /// <param name="target">The GameObject to attempt to damage</param>
        void DoDamage(GameObject target)
        {
            //Check if collision is included in layermask
            if ((damageLayers.value & 1 << target.layer) != 0)
            {
                //Find health component on collided object
                HealthSystem health = target.GetComponent<HealthSystem>();
                if (health)
                {
                    health.Damage(damage);
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            DoDamage(collision.gameObject);
        }

        private void OnCollisionStay(Collision collision)
        {
            DoDamage(collision.gameObject);
        }
    }
}