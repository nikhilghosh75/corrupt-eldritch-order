using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class DisplaySettings : MonoBehaviour
{
    public TextMeshProUGUI fullscreenText;
    public TextMeshProUGUI qualityText;
    public TextMeshProUGUI particlesText;
    public List<string> qualityLevels = new List<string>();

    private int currentQualityIndex;

    private void Start()
    {
        currentQualityIndex = QualitySettings.GetQualityLevel();
        qualityText.text = qualityLevels[currentQualityIndex];

        if (SettingsLoader.Settings.particlesEnabled)
            particlesText.text = "Enabled";
        else
            particlesText.text = "Disabled";
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        if (isFullscreen)
        {
            fullscreenText.text = "Fullscreen";
        }
        else
        {
            fullscreenText.text = "Windowed";
        }
    }

    public void SetQuality(int qualityDelta)
    {
        currentQualityIndex += qualityDelta;
        currentQualityIndex = Mathf.Clamp(currentQualityIndex, 0, qualityLevels.Count - 1);
        qualityText.text = qualityLevels[currentQualityIndex];
        QualitySettings.SetQualityLevel(currentQualityIndex);
    }

    public void SetParticles(bool particlesEnabled)
    {
        Settings settings = SettingsLoader.Settings;
        settings.particlesEnabled = particlesEnabled;
        SettingsLoader.SetSettings(settings);

        if (particlesEnabled)
            particlesText.text = "Enabled";
        else
            particlesText.text = "Disabled";
    }
}
