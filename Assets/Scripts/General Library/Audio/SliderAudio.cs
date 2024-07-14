/*
 * Auguments the behavior of UnityEngine.UI.Slider by adding AudioEvents to when the value changes.
 * @ Jacob Shreve '?, William Bostick '21
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Audio
{
    public class SliderAudio : MonoBehaviour
    {
        private float previousValue;
        [SerializeField] AudioEvent sliderIncrease;
        [SerializeField] AudioEvent sliderDecrease;

        void Start()
        {
            UnityEngine.UI.Slider slider = GetComponent<UnityEngine.UI.Slider>();
            slider.onValueChanged.AddListener(OnValueChanged);
            float value = slider.value;
            previousValue = value;
        }

        /// <summary>
        /// On Value Changed, play the corresponding audio.
        /// </summary>
        /// <param name="value">The current value of the slider</param>
        void OnValueChanged(float value)
        {
            // fire off correct AudioEvent
            if (previousValue > value)
            {
                sliderDecrease.PlayAudio(gameObject);
            }
            else
            {
                sliderIncrease.PlayAudio(gameObject);
            }
            previousValue = value;
        }
    }
}