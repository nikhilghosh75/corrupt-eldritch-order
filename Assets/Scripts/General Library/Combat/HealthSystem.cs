using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WSoft.Combat
{
    /// <summary>
    /// A abstract, inheritable health system. Replacement for the old health script, but designed to be extendable for the needs of a particular game
    /// This health system contains a health value, invincibility functionality, UnityEvents, and abstract Heal and Damage message.
    /// Note that you can pass in objects into heal and damage to provide more information about how to damage the health script.
    /// Written by Nikhil Ghosh '24, Max Perraut '20, 
    /// </summary>
    public abstract class HealthSystem : MonoBehaviour
    {
        /// <summary>
        /// The current health of this health system
        /// Protected so derived classes can directly modify this value but other classes can't
        /// </summary>
        protected int current;

        public int Current { get { return current; } }

        [System.Serializable]
        public struct HealthEvents
        {
            [Tooltip("Invoked when a heal occurs.")]
            public UnityEvent OnHeal;

            [Tooltip("Invoked when damage occurs.")]
            public UnityEvent OnDamage;

            [Tooltip("Invoked when health reaches zero.")]
            public UnityEvent OnDeath;

            [Tooltip("Invoked when any health change occurs")]
            public UnityEvent OnHealthChange;
        }

        [Tooltip("A struct that contains the UnityEvents")]
        public HealthEvents events;

        public bool isInvincible = false;

        bool isDead = false;
        public bool IsDead => isDead;

        void Awake()
        {
            events.OnDamage.AddListener(events.OnHealthChange.Invoke);
            events.OnDeath.AddListener(events.OnHealthChange.Invoke);
            events.OnHeal.AddListener(events.OnHealthChange.Invoke);

            Initialize();
        }

        public bool Heal(int amount, object obj = null)
        {
            if (isDead)
                return false;

            bool healed = ApplyHeal(amount, obj);

            if(healed)
                events.OnHeal.Invoke();

            return healed;
        }

        /// <summary>
        /// The abstract method to apply heal to the health script.
        /// </summary>
        /// <param name="amount">The amount of heal to apply</param>
        /// <param name="obj">An object defining additional data about the heal given</param>
        /// <returns>Did the heal successfully apply</returns>
        protected abstract bool ApplyHeal(int amount, object obj = null);

        public bool Damage(int amount, object obj = null)
        {
            if (isInvincible || isDead)
                return false;

            bool damaged = ApplyDamage(amount, obj);

            if(damaged)
                events.OnDamage.Invoke();

            return damaged;
        }

        /// <summary>
        /// The abstract method to apply damage to the health script.
        /// </summary>
        /// <param name="amount">The amount of damage to apply</param>
        /// <param name="obj">An object defining additional data about the damage taken</param>
        /// <returns>Did the damage successfully apply</returns>
        protected abstract bool ApplyDamage(int amount, object obj = null);

        protected void Die()
        {
            if (isDead)
                return;

            isDead = true;
            events.OnDeath.Invoke();
        }

        /// <summary>
        /// The abstract method to directly set the health of a system. Used primarily for debugging purposes, but 
        /// </summary>
        /// <param name="amount">The amount of health to set it to</param>
        public void SetHealth(int amount)
        {
            current = amount;
            events.OnHealthChange.Invoke();
        }

        /// <summary>
        /// Initialize the health script. Called during Awake
        /// Designed to avoid the issues with MonoBehavior inheritance
        /// </summary>
        protected virtual void Initialize() { }

        /// <summary>
        /// A way to get debug data for the inspector to display
        /// </summary>
        /// <returns>A string that the inspector will show</returns>
        public virtual string GetDebugData()
        {
            return "Current: " + current;
        }
    }
}