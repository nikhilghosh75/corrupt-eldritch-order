using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WSoft.Combat
{
    /// <summary>
    /// A health system with a max health and a number of "lives". 
    /// The player does not call the "Die()" method until all lives are lost
    /// Based off of the health script from Twin Blades Vengeance
    /// Written by Nikhil Ghosh '24, Alvin Hermans '22, Bri Epstein '22
    /// </summary>
    public class LivesHealth : HealthSystem
    {
        [Tooltip("The maximum amount of health. Also the starting amount")]
        public int maxHealth;

        [Tooltip("The maximum amount of lifes. Also the starting amount")]
        public int maxLifes;

        int currentLifes;
        public int CurrentLifes { get { return currentLifes; } }

        public UnityEvent OnLifeLost;
        public UnityEvent OnLifeGained;

        protected override void Initialize()
        {
            base.Initialize();

            currentLifes = maxLifes;
            current = maxHealth;
        }

        protected override bool ApplyDamage(int amount, object obj = null)
        {
            current -= amount;
            if(current <= 0)
            {
                currentLifes--;
                if(currentLifes < 0)
                {
                    current = 0;
                    Die();
                }
                else
                {
                    current = maxHealth;
                    OnLifeLost.Invoke();
                }
            }

            return true;
        }

        protected override bool ApplyHeal(int amount, object obj = null)
        {
            current += amount;

            if(current > maxHealth)
            {
                int additionalHealth = current - maxHealth;
                current = additionalHealth % maxHealth;
                currentLifes += (additionalHealth / maxHealth) + 1;

                if (currentLifes > maxLifes)
                    currentLifes = maxLifes;

                OnLifeGained.Invoke();
            }

            return true;
        }

        public override string GetDebugData()
        {
            return "Current: " + current + "/" + maxHealth + "\nLives: " + currentLifes + "/" + maxHealth;
        }
    }
}
