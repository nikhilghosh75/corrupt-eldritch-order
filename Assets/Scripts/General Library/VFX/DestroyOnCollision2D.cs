using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Math;

/*
 * Destroys itself on collision with a certain layermask
 * Written by Nikhil Ghosh '24
 */

namespace WSoft
{
    public class DestroyOnCollision2D : MonoBehaviour
    {
        [Tooltip("The layermask to determine if it should be destroyed")]
        public LayerMask layerMaskToDestroy;

        [Tooltip("The game object to be spawned on trigger. Set to null for nothing to spawn")]
        public GameObject hitParticle;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (LayermaskFunctions.IsInLayerMask(layerMaskToDestroy, collision.gameObject.layer))
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