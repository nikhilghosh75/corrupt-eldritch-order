/* 
 * Destroys gameobject on trigger enter.
 * Optionally instantiates hit particle prefab when destroyed.
 * @ Max Perraut '20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.VFX
{
    public class DestroyOnTrigger : MonoBehaviour
    {
        [Tooltip("The layermask to determine if the object should be destroyed")]
        public LayerMask layerMaskToDestroy;

        [Tooltip("The game object to be spawned on trigger. Set to null for nothing to spawn")]
        public GameObject hitParticle;

        private void OnTriggerEnter(Collider collision)
        {
            if (Math.LayermaskFunctions.IsInLayerMask(layerMaskToDestroy, collision.gameObject.layer))
            {
                if (hitParticle)
                {
                    Instantiate(hitParticle, this.transform.position, Quaternion.identity);
                }
                Destroy(gameObject);
            }
        }
    }
}
