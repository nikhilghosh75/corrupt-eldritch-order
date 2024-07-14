/*
 * A hook for updating a UI component based on the players health (in the backend)
 * Depends on: Some HealthUIManager script
 * @ Original author Unknown. Added by Nigel Charleston '21, See list of original team programmers at https://wolverinesoft-studio.itch.io/bloom-tome-of-power
 * 
 * This code may have aspects/assumptions that were specific to its original project. 
 * I would recommend using it as a reference (when implementing a new script), rather than purely copying it and pasting it
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WSoft.FirstPerson
{
    public class PlayerHealthUIHook : MonoBehaviour
    {
        int lastHealth;
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            SetText(GetComponent<Health>().Current);
        }
        public void SetText(int health)
        {
            HealthUIManager.instance.SetHealthNumber(health);
            if (health > lastHealth)
            {
                HealthUIManager.instance.TriggerHealthUIAnim();
            }
            lastHealth = health;
        }
    }
}
