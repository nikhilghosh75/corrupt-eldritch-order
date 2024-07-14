/*
 * This script controls particle system by spawning particles upon enabling
 * Attach this script directly onto the projectile being spawned
 * Originally designed for the boss system, but has been more generalized
 * @ David Sohn '20
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.VFX
{
    public class SpawnParticleOnEnable : MonoBehaviour
    {
        [Tooltip("The delay when killing the primary particle systems")]
        [SerializeField] private float killDelay;

        [Tooltip("Should the particles be parented to the object spawning it")]
        [SerializeField] private bool parentToObject;

        [Tooltip("A list of particles to be initially spawned")]
        [SerializeField] private List<GameObject> initialParticles;

        [Tooltip("A list of references to the primary particles spawned in the game world")]
        private List<GameObject> initialParticlesReferences;

        [Tooltip("The offset of the primary particles to be spawned from the game object")]
        [SerializeField] private Vector3 initialParticleOffset;

        [Space]

        [Tooltip("Should the script spawn secondary particles")]
        [SerializeField] private bool spawnSecondaryParticles;

        [Tooltip("The delay when killing the secondary particle systems, if secondary particles are to be spawned.")]
        [SerializeField] private float secondaryKillDelay;

        [Tooltip("Should the secondary particle system be parented to the object spawning it")]
        [SerializeField] private bool parentToObjectSecondary;

        [Tooltip("A list of secondary particles to be initially spawned")]
        [SerializeField] private List<GameObject> secondaryParticles;

        [Tooltip("A list of references to the secondary particles spawned in the game world")]
        private List<GameObject> secondaryParticlesReferences;

        [Tooltip("The offset of the secondary particles to be spawned from the game object")]
        [SerializeField] private Vector3 secondaryParticleOffset;

        void Awake()
        {
            initialParticlesReferences = new List<GameObject>();
            secondaryParticlesReferences = new List<GameObject>();
        }

        public void OnEnable()
        {
            foreach (GameObject particle in initialParticles)
            {
                GameObject newParticle;
                if (parentToObject)
                {
                    newParticle = Instantiate(particle, transform.position + initialParticleOffset, Quaternion.identity);
                    newParticle.transform.parent = transform;
                }
                else
                {
                    newParticle = Instantiate(particle, transform.position + initialParticleOffset, Quaternion.identity);
                }
                initialParticlesReferences.Add(newParticle);
                newParticle.transform.parent = this.gameObject.transform;
            }
        }


        /// <summary>
        /// Spawns the secondary particles.  Activate with the event in the boss projectile.
        /// </summary>
        public void SpawnParticles()
        {
            foreach (GameObject particle in secondaryParticles)
            {
                GameObject newParticle;
                if (parentToObjectSecondary)
                {
                    newParticle = Instantiate(particle, transform.position + secondaryParticleOffset,
                     Quaternion.identity);
                    newParticle.transform.parent = transform;
                }
                else
                    newParticle = Instantiate(particle, transform.position + secondaryParticleOffset,
                     Quaternion.identity);
                secondaryParticlesReferences.Add(newParticle);
                newParticle.transform.parent = this.gameObject.transform;
            }
        }

        /// <summary>
        /// Removes particles.  Activate with the event in the boss projectile.
        /// </summary>
        public void KillPrimaryParticles()
        {
            StartCoroutine(KillParticlesAfterDelay(true));
        }

        /// <summary>
        /// Kill all the secondary particles
        /// </summary>
        public void KillSecondaryParticles()
        {
            StartCoroutine(KillParticlesAfterDelay(false));
        }

        IEnumerator KillParticlesAfterDelay(bool killPrimary)
        {
            if (killPrimary)
            {
                yield return new WaitForSeconds(killDelay);
                foreach (GameObject particle in initialParticlesReferences)
                {
                    if (particle != null)
                        Destroy(particle);
                }
            }
            else
            {
                yield return new WaitForSeconds(secondaryKillDelay);
                if (spawnSecondaryParticles)
                {
                    foreach (GameObject particle in secondaryParticlesReferences)
                    {
                        if (particle != null)
                            Destroy(particle);
                    }
                }
            }
        }
    }
}
