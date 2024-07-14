/* Stores the display name and control sprite of an input action.
 * @Zhenyuan Zhang '21
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;

namespace WSoft.Input.ControlPrompt
{
    [CreateAssetMenu(fileName = "New Control Prompt", menuName = "WSoft/Control Prompt/Control Prompt")]
    public class ControlPrompt : ScriptableObject
    {
        public string actionName;
        public string displayName;
        public Sprite keyboardSprite;
        public Sprite playstationSprite;
        public Sprite xboxSprite;
        public Sprite nintendoSprite;

        public Sprite Sprite
        {
            get
            {
                if (Gamepad.current == null)
                    return keyboardSprite;

                if (Gamepad.current is DualShockGamepad)
                    return playstationSprite;
                if (Gamepad.current is XInputController)
                    return xboxSprite;
                if (Gamepad.current is SwitchProControllerHID)
                    return nintendoSprite;

                return null;

            }
        }
    }

}