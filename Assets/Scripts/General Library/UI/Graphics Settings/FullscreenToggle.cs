/*
 * Handles the fullscreen toggle in the options
 * 
 * @ Bri Epstein '22
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WSoft.UI.GraphicsSettings
{
    public class FullscreenToggle : MonoBehaviour
    {
        Toggle toggle;
        bool fullscreen = false;

        GraphicsSettingsManager graphicsManager;

        // Start is called before the first frame update
        void Start()
        {
            toggle = GetComponent<Toggle>();
            graphicsManager = GraphicsSettingsManager.Get();

            Screen.fullScreen = toggle.isOn = graphicsManager.fullscreenOption;
        }

        public void onValueChanged(Toggle changed)
        {
            graphicsManager.fullscreenOption = fullscreen = changed.isOn;
            Screen.SetResolution(Screen.width, Screen.height, fullscreen);
        }
    }
}