using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*
 * A component that is added to a selectable that allows the slider's value to be controlled by the gamepad's right stick
 * Written by Nikhil Ghosh '24
 */

namespace WSoft.Input
{
    public class InputSlider : MonoBehaviour
    {
        public Slider slider;
        public float sliderSpeed;

        bool isSelected = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (isSelected)
            {
                if (Gamepad.current != null)
                {
                    float x = Gamepad.current.rightStick.ReadValue().x;
                    slider.value = Mathf.Clamp(slider.value + x * sliderSpeed * Time.deltaTime, slider.minValue, slider.maxValue);
                }
            }
        }

        public void OnSelect()
        {
            isSelected = true;
        }

        public void OnDeselect()
        {
            isSelected = false;
        }
    }
}