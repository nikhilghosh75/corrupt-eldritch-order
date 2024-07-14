using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimingToggle : MonoBehaviour
{
    public Button eightDirectionButton;
    public Button continousAimButton;

    public Color selectedColor;
    public Color unselectedColor;

    // Start is called before the first frame update
    void Start()
    {
        UpdateDisplay();
        EventBus.Subscribe<SettingsChangedEvent>(OnSettingsChanged);

        eightDirectionButton.onClick.AddListener(SelectEightDirectional);
        continousAimButton.onClick.AddListener(SelectContinous);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateDisplay()
    {
        bool eightDirectionalEnabled = SettingsLoader.Settings.eightDirectionalEnabled;
        eightDirectionButton.image.color = eightDirectionalEnabled ? selectedColor : unselectedColor;
        continousAimButton.image.color = eightDirectionalEnabled ? unselectedColor : selectedColor;
    }

    void OnSettingsChanged(SettingsChangedEvent e)
    {
        UpdateDisplay();
    }

    public void SelectEightDirectional()
    {
        Settings settings = SettingsLoader.Settings;
        settings.eightDirectionalEnabled = true;
        SettingsLoader.SetSettings(settings);
    }

    public void SelectContinous()
    {
        Settings settings = SettingsLoader.Settings;
        settings.eightDirectionalEnabled = false;
        SettingsLoader.SetSettings(settings);
    }
}
