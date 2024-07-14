using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WSoft.Math;

/*
 * Handles when objects enter the enemy's radius and make the enemy "aggroed"
 * Useful for activating/deactivating enemies
 * Can be used on player-friendly characters as well
 * Written by Nikhil Ghosh '24
 */

namespace WSoft.Combat
{
    public class EnemyAggro : MonoBehaviour
    {
        [Tooltip("A layermask of objects that can actually aggro the enemy")]
        public LayerMask aggroLayerMask;

        public AggroEvent OnAggro;
        public UnityEvent OnDeaggro;

        GameObject target;

        // Start is called before the first frame update
        void Start()
        {
            // All aggro colliders should be triggers, not colliders
            Collider collider = GetComponent<Collider>();
            collider.isTrigger = true;
        }

        public GameObject GetTarget() { return target; }

        void OnTriggerEnter(Collider collision)
        {
            if (LayermaskFunctions.IsInLayerMask(aggroLayerMask, collision.gameObject.layer))
            {
                target = collision.gameObject;
                OnAggro.Invoke(target);
            }
        }

        void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject == target)
            {
                target = null;
                OnDeaggro.Invoke();
            }
        }
    }
}

