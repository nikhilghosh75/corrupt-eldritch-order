/* 
 * Applies damage to objects on trigger collision, use a layermask
 * @ Max Perraut '20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WSoft.Combat
{
    public class DamageOnTrigger2D : MonoBehaviour
    {
        [System.Serializable]
        public class DamageEvent : UnityEvent<GameObject> { }

        [Tooltip("The amount of damage that should be inflicted")]
        public int damage;

        [Tooltip("Only damages objects on these layers.")]
        public LayerMask damageLayers;

        [Tooltip("Should we damage on stay")]
        public bool damageOnStay;

        public DamageEvent OnDamageCaused;

        /// <summary>
        /// On a trigger enter, find a health component on the specified object and damage.
        /// </summary>
        /// <param name="target">The GameObject to attempt to damage</param>
        private void DoDamage(GameObject target)
        {
            //Check if collision is included in layermask
            if ((damageLayers.value & 1 << target.layer) != 0)
            {
                //Find health component on collided object
                HealthSystem health = target.GetComponent<HealthSystem>();
                if (health)
                {
                    bool damageCaused = health.Damage(damage);
                    if (damageCaused)
                        OnDamageCaused.Invoke(target);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            DoDamage(collision.gameObject);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (damageOnStay)
                DoDamage(collision.gameObject);
        }
    }
}

