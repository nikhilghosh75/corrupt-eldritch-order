using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AccessibilitySettings : MonoBehaviour
{
    public TextMeshProUGUI screenshakeText;

    // Start is called before the first frame update
    void Start()
    {
        if (SettingsLoader.Settings.screenshakeEnabled)
            screenshakeText.text = "Enabled";
        else
            screenshakeText.text = "Disabled";
    }

    public void SetScreenshake(bool screenshakeEnabled)
    {
        Settings settings = SettingsLoader.Settings;
        settings.screenshakeEnabled = screenshakeEnabled;
        SettingsLoader.SetSettings(settings);

        if (SettingsLoader.Settings.screenshakeEnabled)
            screenshakeText.text = "Enabled";
        else
            screenshakeText.text = "Disabled";
    }
}
