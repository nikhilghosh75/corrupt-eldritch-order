/*
 * Controls the Controller Vibration using the Unity Input system
 * Note that all methods are static and no controller vibration should ever be called during a scene transition
 * @ Matt Rader '19, Nikhil Ghosh '24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WSoft.Tools.Console;

namespace WSoft.Input
{
    [System.Serializable]
    public struct VibrationSettings
    {
        [Tooltip("This sets the motor speed for the controller. Must be a value between 0 and 1.")]
        public float vibrationIntensity;

        [Tooltip("Length of the vibration in seconds")]
        public float vibrationDuration;
    }

    public class ControllerVibration
    {
        /// <summary>
        /// Vibrates the current controller, if it exists
        /// </summary>
        /// <param name="settings">The settings of the vibration</param>
        public static void VibrateController(VibrationSettings settings)
        {
            if(Gamepad.current == null)
                return;

            DeveloperConsoleBehaviour devConsole = DeveloperConsoleBehaviour.Instance;

            if (devConsole == null)
            {
                Debug.LogError("Developer Console does not exist in current scene");
                return;
            }
            devConsole.StartCoroutine(VibrateControllerCoroutine(Gamepad.current, settings));
        }

        /// <summary>
        /// Vibrates the controller at the specified index, if it exists
        /// </summary>
        /// <param name="index">The index to vibrate the controller</param>
        /// <param name="settings">The settings of the vibration</param>
        public static void VibrateController(int index, VibrationSettings settings)
        {
            UnityEngine.InputSystem.Utilities.ReadOnlyArray<Gamepad> pads = Gamepad.all;
            if (index >= pads.Count)
            {
                Debug.LogError("Controller at index " + index + " not found.");
                return;
            }
            Gamepad currentGamepad = pads[index];

            DeveloperConsoleBehaviour devConsole = DeveloperConsoleBehaviour.Instance;

            if (devConsole == null)
            {
                Debug.LogError("Developer Console does not exist in current scene");
                return;
            }
            devConsole.StartCoroutine(VibrateControllerCoroutine(currentGamepad, settings));
        }

        /// <summary>
        /// Pause the controller vibration. Useful for pause menus or other functions where Haptics must stop.
        /// </summary>
        public static void PauseControllerVibration()
        {
            InputSystem.PauseHaptics();
        }

        /// <summary>
        /// Resumes the controller vibration if it was paused.
        /// </summary>
        public static void ResumeControllerVibration()
        {
            InputSystem.ResumeHaptics();
        }

        static IEnumerator VibrateControllerCoroutine(Gamepad pad, VibrationSettings settings)
        {
            pad.SetMotorSpeeds(settings.vibrationIntensity, settings.vibrationIntensity);
            yield return new WaitForSeconds(settings.vibrationDuration);
            pad.SetMotorSpeeds(0f, 0f);
        }
    }
}
