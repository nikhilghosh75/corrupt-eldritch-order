using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WSoft.Combat
{
    public class DamageInfo
    {
        public string damageType;
        public GameObject attacker;
    }

    /// <summary>
    /// A health script with iFrames, a max health, and a list of damage types that the health is immune to.
    /// Based off the health system from Ragnarok TD
    /// </summary>
    public class BasicHealthWithImmunities : HealthSystem
    {
        [System.Serializable]
        public class IframeDurationEvent : UnityEvent<float> { }

        [Tooltip("The maximum amount of health. Also the starting amount")]
        public int maxHealth;

        [Tooltip("Are invincibility frames enabled when the health takes damage")]
        public bool iframesEnabled = false;

        [Tooltip("The duration of the iFrames, if iFrames are enabled")]
        public float iframesDuration = 1f;

        [Tooltip("The types of damages that this health is immune to")]
        public List<string> damagesImmuneTo;

        // The time at which the iFrames end, in Unity time
        private float iframesEnd = -1f;

        [Tooltip("Invoked when iframes block damage. Passed the duration of iframes left.")]
        public IframeDurationEvent OnIframes;

        protected override void Initialize()
        {
            base.Initialize();

            current = maxHealth;
        }

        protected override bool ApplyDamage(int amount, object obj = null)
        {
            if(obj != null && obj is DamageInfo)
            {
                DamageInfo damageInfo = (DamageInfo)obj;
                if (damagesImmuneTo.Contains(damageInfo.damageType))
                    return false;
            }

            if (iframesEnabled)
            {
                // Black damage if the iframes are stil active
                if (Time.time < iframesEnd)
                {
                    OnIframes.Invoke(iframesEnd - Time.time);
                    return false;
                }

                iframesEnd = Time.time + iframesDuration;
            }

            current -= amount;

            if (current <= 0)
            {
                current = 0;
                Die();
            }

            return true;
        }

        protected override bool ApplyHeal(int amount, object obj = null)
        {
            current += amount;

            if (current > maxHealth)
                current = maxHealth;

            return true;
        }

        public override string GetDebugData()
        {
            string str = "Current: " + current + "/" + maxHealth + "\nImmunities:";
            foreach (string damage in damagesImmuneTo)
                str += " " + damage;

            return str;
        }
    }

}