/* 
 * Despawns a gameobject after a specified amount of time.
 * @ Alex Kisil '19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.VFX
{
    public class Despawn : MonoBehaviour
    {
        [Tooltip("Automatically Despawn after a certain amount of time")]
        public bool autoDespawn = false;

        [Tooltip("The time between start and destroy, in seconds, if autoDespawn is true")]
        public float timeToAutoDespawn = 5f;

        [Tooltip("The time between the Early Despawn call and the actual destruction of the object, in seconds")]
        public float earlyDespawnTime = 3f;

        void Start()
        {
            if (autoDespawn)
            {
                StartCoroutine(Predespawn(timeToAutoDespawn));
            }
        }

        /// <summary>
        /// Destroy the object before the automatic despawn time
        /// Note that this function does not halt the automatic destruction of the object, if autoDespawn is set to true.
        /// </summary>
        public void EarlyDespawn()
        {
            StartCoroutine(Predespawn(earlyDespawnTime));
        }

        IEnumerator Predespawn(float despawnTime)
        {
            while (true)
            {
                // Suspend execution for despawnTime seconds
                yield return new WaitForSeconds(despawnTime);
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }
    }

}