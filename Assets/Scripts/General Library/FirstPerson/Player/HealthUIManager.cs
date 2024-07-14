/*
 * A script that manages the UI that displays information relevate to a player's health. 
 * Requires: animator, TMP_Text
 * @ Original author Unknown. Added by Nigel Charleston '21, See list of original team programmers at https://wolverinesoft-studio.itch.io/bloom-tome-of-power
 * 
 * This code may have aspects/assumptions that were specific to its original project. 
 * I would recommend using it as a reference (when implementing a new script), rather than purely copying it and pasting it
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace WSoft.FirstPerson
{
    public class HealthUIManager : MonoBehaviour
    {
        public static HealthUIManager instance;
        [SerializeField] TMP_Text myHealthCounter;
        [SerializeField] Animator healthIconAnim;

        public /// <summary>
               /// Awake is called when the script instance is being loaded.
               /// </summary>
    void Awake()
        {
            instance = this;
        }

        public void SetHealthNumber(int newNumber)
        {
            myHealthCounter.text = newNumber.ToString();
        }

        public void TriggerHealthUIAnim()
        {
            healthIconAnim.SetTrigger("GainAmmo");
        }
    }
}