using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Regenerates a health when not at max health
 * Written by Rex Ma '22
 */

namespace WSoft.Combat
{
    public class EnemyRegen : MonoBehaviour
    {
        [Tooltip("Whether regeneration will occur")]
        public bool enableRegeneration = true;
        [Tooltip("The time between when the enemy recovers health")]
        public float timeBetweenRegen = 1.5f;
        [Tooltip("The amount of health the enemy heals")]
        public int regenAmount = 1;

        private HealthSystem health;

        private void Start()
        {
            health = GetComponent<HealthSystem>();
            StartCoroutine(HealthRegeneration());
        }

        private IEnumerator HealthRegeneration()
        {
            while (true)
            {
                yield return new WaitForSeconds(timeBetweenRegen);
                if (enableRegeneration)
                {
                    health.Heal(regenAmount);
                }
            }
        }
    }
}