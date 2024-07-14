/*
 * Augments the behavior of UnityEngine.UI.Slider by changing the text of a TextComponent to match the value.
 * @ Jacob Shreve '?, Nikhil Ghosh '23
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WSoft.UI
{
    public class SliderText : MonoBehaviour
    {
        [Tooltip("The text to be modified")]
        public TMP_Text valueText;

        public float scaleFactor = 1f;
        public bool isInteger = false;

        void Start()
        {
            UnityEngine.UI.Slider slider = GetComponent<UnityEngine.UI.Slider>();
            slider.onValueChanged.AddListener(OnValueChanged);
            valueText.text = ValueToString(slider.value);
        }

        void OnValueChanged(float value)
        {
            valueText.text = ValueToString(value);
        }

        string ValueToString(float value)
        {
            if (isInteger)
                return ((int)(value * scaleFactor)).ToString();

            return (value * scaleFactor).ToString();
        }
    }
}