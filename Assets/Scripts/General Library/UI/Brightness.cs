/*
 * A framework for a brightness slider
 * Written by Natasha Badami '20. Added by Nigel Charleston '21, See list of original team programmers at https://wolverinesoft-studio.itch.io/bloom-tome-of-power
 * 
 * This code may have aspects/assumptions that were specific to its original project. 
 * I would recommend using it as a reference (when implementing a new script), rather than purely copying it and pasting it
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WSoft.UI
{
    public class Brightness : MonoBehaviour
    {
        [SerializeField] Slider brightnessSlider;
        Image brightnessOverlay;


        void Awake()
        {
            brightnessOverlay = GetComponent<UnityEngine.UI.Image>();

            if (PlayerPrefs.HasKey("brightness"))
            {
                SetBrightness(PlayerPrefs.GetFloat("brightness"));
            }
            else
            {
                SetBrightness(1);
            }
        }

        /// <summary>
        /// Sets the brightness from a given slider value
        /// </summary>
        /// <param name="slider">The slider to take the normalized value of</param>
        public void SetBrightnessFromSlider(Slider slider)
        {
            SetBrightness(slider.normalizedValue);
        }

        /// <summary>
        /// Set the screen brightness
        /// </summary>
        /// <param name="value">A float between 0 and 1</param>
        public void SetBrightness(float value)
        {
            value = Mathf.Clamp01(value);
            Debug.Log("brightness set to " + value);
            PlayerPrefs.SetFloat("brightness", value);
            brightnessSlider.SetValueWithoutNotify(value * brightnessSlider.maxValue);

            float brightness = .5f + value / 2;
            Color c = brightnessOverlay.color;
            c.a = 1 - brightness;
            brightnessOverlay.color = c;
        }
    }
}

