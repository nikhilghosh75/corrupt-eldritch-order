/*
 * A script that serves as the backend for a players health (how its represented under the hood)
 * Written by Natasha Badami '20, Matt Rader '19, Evan Brisita '18
 * 
 * This code may have aspects/assumptions that were specific to its original project. 
 * I would recommend using it as a reference (when implementing a new script), rather than purely copying it and pasting it
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

namespace WSoft.FirstPerson
{
    public class Health : MonoBehaviour
    {

        [SerializeField]
        private int current;
        public int Current { get { return current; } }

        public int max;

        [Header("I-Frames")]
        public bool iframesEnabled = false;
        public float iframesDuration = 1f;

        public bool hasDefenseBuff { get; set; } = false;

        //Wrap all events in a struct to group them in Editor
        [System.Serializable]
        public struct HealthEvents
        {
            [System.Serializable]
            public class HealthValueEvent : UnityEvent<int> { }

            [System.Serializable]
            public class IframeDurationEvent : UnityEvent<float> { }

            [Tooltip("Invoked when a heal occurs.")]
            public UnityEvent OnHeal;

            [Tooltip("Invoked when damage occurs.")]
            public UnityEvent OnDamage;

            [Tooltip("Invoked when iframes are triggered. Passed the duration of iframes.")]
            public IframeDurationEvent OnIframes;

            [Tooltip("Invoked when health changes. It is passed the value of health after the change.")]
            public HealthValueEvent OnHealthChange;

            [Tooltip("Invoked when health reaches zero.")]
            public UnityEvent OnDeath;
        }

        public HealthEvents events;

        private float iframesEnd = 0f;

        private bool dying;

        private void Start()
        {
            if (current != max) Debug.Log("Starting at less than maximum health.");

            // sus code
            // if (name == "Player")
                // events.OnHealthChange.AddListener(GameStateEvents.OnHealthChange);
        }

        /// <summary>
        /// Adds health up to the maximum
        /// </summary>
        public void Heal(int amount)
        {
            //Assumes if being healed, entity is not dead
            dying = false;
            current += amount;

            if (current > max) current = max;

            current = Mathf.Clamp(current, 0, max);

            events.OnHealthChange.Invoke(current);
            events.OnHeal.Invoke();
        }

        /// <summary>
        /// Applies damage
        /// </summary>
        public void ApplyDamage(int amount)
        {
            // do iframes if enabled
            if (iframesEnabled)
            {
                // block damage if in iframe time
                if (Time.time < iframesEnd) return;

                // start iframes
                iframesEnd = Time.time + iframesDuration;

                events.OnIframes.Invoke(iframesDuration);
            }

            if (!hasDefenseBuff)
            {
                current -= amount;
            }

            // Make sure that we're not already dying before trying to die
            if (current <= 0 && !dying)
            {
                dying = true;
                current = 0;
                events.OnDeath.Invoke();
            }

            current = Mathf.Clamp(current, 0, max);

            events.OnHealthChange.Invoke(current);
            events.OnDamage.Invoke();
        }
    }
}