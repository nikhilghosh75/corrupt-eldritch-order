/*
 * Handles the quality settings dropdown in the options
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
    public class QualityDropdown : MonoBehaviour
    {
        TMP_Dropdown dropdown;
        GraphicsSettingsManager graphicsManager;

        void Start()
        {
            dropdown = GetComponent<TMP_Dropdown>();
            graphicsManager = GraphicsSettingsManager.Get();

            //populate dropdown menu with the quality levels from the project
            string[] names = QualitySettings.names;
            foreach (string name in names)
            {
                TMP_Dropdown.OptionData newData = new TMP_Dropdown.OptionData();
                newData.text = name;
                dropdown.options.Add(newData);
            }
            dropdown.RefreshShownValue();

            // If graphics quality option is already set, we don't need to set it
            if (graphicsManager.qualityOption != -1)
            {
                dropdown.value = graphicsManager.qualityOption;
                QualitySettings.SetQualityLevel(graphicsManager.qualityOption, true);
            }
            // If graphics quality is not set, set it to the maximum value
            else
            {
                dropdown.value = names.Length - 1;
                graphicsManager.qualityOption = names.Length - 1;
                QualitySettings.SetQualityLevel(names.Length - 1, true);

            }
            dropdown.RefreshShownValue();
            dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        public void OnValueChanged(int value)
        {
            //update quality setting
            graphicsManager.qualityOption = value;
            QualitySettings.SetQualityLevel(value, true);
            dropdown.RefreshShownValue();
        }
    }
}