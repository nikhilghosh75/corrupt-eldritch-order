/*
 * Handles the resolution dropdown in the options
 * 
 * @ Bri Epstein '22
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WSoft.UI.GraphicsSettings
{
    public class ResolutionDropdown : MonoBehaviour
    {
        TMP_Dropdown dropdown;

        Resolution[] options;
        GraphicsSettingsManager graphicsManager;

        void Start()
        {
            dropdown = GetComponent<TMP_Dropdown>();
            graphicsManager = GraphicsSettingsManager.Get();

            //show resolutions supported by the monitor 
            dropdown.options.Clear();

            options = graphicsManager.FindAllowedResolutions();
            foreach (Resolution r in options)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(r.width + " x " + r.height));
            }

            if (graphicsManager.resolutionOption != -1)
            {
                dropdown.value = graphicsManager.resolutionOption;
            }
            else
            {
                graphicsManager.DefaultToHighestRes();
                dropdown.value = options.Length;
            }
            dropdown.onValueChanged.AddListener(OnValueChanged);
            OnValueChanged(dropdown.value);
            dropdown.RefreshShownValue();
        }

        public void OnValueChanged(int value)
        {
            dropdown.value = graphicsManager.resolutionOption = value;
            Screen.SetResolution(options[value].width, options[value - 1].height, Screen.fullScreenMode);
            dropdown.RefreshShownValue();
        }
    }
}