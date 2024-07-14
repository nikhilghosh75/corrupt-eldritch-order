using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/*
 * A set of general coroutine utilities that can be used anywhere
 * Includes various coroutines, stored in the CoroutineUtilities class, along with some custom yield instructions
 * Written by Nikhil Ghosh '24
 */

namespace WSoft
{
    public class CoroutineUtilities : MonoBehaviour
    {
        /// <summary>
        /// A coroutine that waits until the mouse is clicked
        /// </summary>
        /// <param name="mouseButton">0 for left click, 1 for right click, 2 for middle click</param>
        public static IEnumerator WaitForClick(int mouseButton = 0)
        {
            while (true)
            {
                switch (mouseButton)
                {
                    case 0:
                        if (Mouse.current.leftButton.wasPressedThisFrame)
                            yield break;
                        break;
                    case 1:
                        if (Mouse.current.rightButton.wasPressedThisFrame)
                            yield break;
                        break;
                    case 2:
                        if (Mouse.current.middleButton.wasPressedThisFrame)
                            yield break;
                        break;
                }
                yield return null;
            }
        }
    }

    /// <summary>
    /// A custom yield instruction that waits until a UI button is clicked
    /// Called by "yield return new WaitForButtonPress(button);
    /// </summary>
    public class WaitForButtonPress : CustomYieldInstruction
    {
        bool isClicked = false;

        public WaitForButtonPress(Button button)
        {
            button.onClick.AddListener(OnButtonClick);
        }

        public override bool keepWaiting
        {
            get
            {
                return !isClicked;
            }
        }

        void OnButtonClick()
        {
            isClicked = true;
        }
    }


}