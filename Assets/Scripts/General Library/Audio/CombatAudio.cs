using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Combat;

/*
 * A class for helping with combat based audio, such as
 * audio on damage, heal, and death events
 * Note that the
 * Written by Nikhil Ghosh '24
 */

namespace WSoft.Audio
{
    [RequireComponent(typeof(HealthSystem))]
    public class CombatAudio : MonoBehaviour
    {
        [Tooltip("The audio to be played when the health is damaged")]
        public AudioEvent damageAudioEvent;

        [Tooltip("The audio to be played when the health is healed")]
        public AudioEvent healAudioEvent;

        [Tooltip("The audio to be played when the health dies")]
        public AudioEvent deathAudioEvent;

        // Start is called before the first frame update
        void Start()
        {
            HealthSystem health = GetComponent<HealthSystem>();
            health.events.OnDamage.AddListener(OnDamage);
            health.events.OnHeal.AddListener(OnHeal);
            health.events.OnDeath.AddListener(OnDeath);
        }

        void OnDamage()
        {
            if (damageAudioEvent != null)
                damageAudioEvent.PlayAudio(gameObject);
        }

        void OnHeal()
        {
            if (healAudioEvent != null)
                healAudioEvent.PlayAudio(gameObject);
        }

        void OnDeath()
        {
            if (deathAudioEvent != null)
                deathAudioEvent.PlayAudio(gameObject);
        }
    }
}