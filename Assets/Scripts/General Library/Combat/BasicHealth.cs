using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WSoft.Combat
{
    /// <summary>
    /// A basic health system with iFrames and a max health implemented
    /// Written by Nikhil Ghosh '24, Max Perraut '20
    /// </summary>
    public class BasicHealth : HealthSystem
    {
        [System.Serializable]
        public class IframeDurationEvent : UnityEvent<float> { }

        [Tooltip("The maximum amount of health. Also the starting amount")]
        public int maxHealth;

        [Tooltip("Are invincibility frames enabled when the health takes damage")]
        public bool iframesEnabled = false;

        [Tooltip("The duration of the iFrames, if iFrames are enabled")]
        public float iframesDuration = 1f;

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
            if(iframesEnabled)
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
            
            if(current <= 0)
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

        protected bool IsInIFrames()
        {
            return Time.time < iframesEnd;
        }

        public override string GetDebugData()
        {
            return "Current: " + current + "/" + maxHealth +
                "\n" + (Time.time < iframesEnd ? "iFrames active" : "iFrames inactive");
        }
    }
}