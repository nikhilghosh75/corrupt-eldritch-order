using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

/*
 * A script that is attached to a button that allows its onclick functionality to work when any keypress or gamepad button press
 * is performed. Useful for the start of a main menu
 * Written by Nikhil Ghosh '24
 */

namespace WSoft.Input
{
    public class PressAnythingButton : MonoBehaviour
    {
        Button button;

        // Start is called before the first frame update
        void Start()
        {
            button = GetComponent<Button>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Gamepad.current != null)
            {
                bool gamepadButtonPressed = Gamepad.current.allControls.Any(x => x is ButtonControl button && button.isPressed && !x.synthetic);
                if (gamepadButtonPressed)
                    button.Select();
            }
            else
            {
                if (Keyboard.current.anyKey.isPressed)
                    button.Select();
            }
        }
    }
}