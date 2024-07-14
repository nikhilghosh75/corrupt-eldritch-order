using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WSoft.Core;

[Serializable]
public class Settings
{
    public bool eightDirectionalEnabled;
    public bool particlesEnabled;
    public bool screenshakeEnabled;

    public int masterVolume;
    public int musicVolume;
    public int sfxVolume;
    public int dialogueVolume;
    public int enemiesVolume;
    public int playerVolume;
    public int uiVolume;
}

public class SettingsChangedEvent
{
    public Settings settings;

    public SettingsChangedEvent(Settings _settings)
    {
        settings = _settings;
    }
}

public class SettingsLoader : MonoBehaviour
{
    static Settings settings;

    public static Settings Settings { get { return settings; } }

    void Awake()
    {
        if (File.Exists(SaveManager.GetPath("settings")))
        {
            settings = FixSettings(SaveManager.Load<Settings>("settings"));

            if (settings == null)
            {
                SetSettings(GetDefaultSettings());
            }
        }
        else
        {
            SetSettings(GetDefaultSettings());
        }
    }

    static Settings GetDefaultSettings()
    {
        Settings defaultSettings = new Settings();

        defaultSettings.eightDirectionalEnabled = false;
        defaultSettings.particlesEnabled = true;
        defaultSettings.screenshakeEnabled = true;

        defaultSettings.masterVolume = 5;
        defaultSettings.musicVolume = 5;
        defaultSettings.sfxVolume = 5;
        defaultSettings.dialogueVolume = 5;
        defaultSettings.enemiesVolume = 5;
        defaultSettings.playerVolume = 5;
        defaultSettings.uiVolume = 5;

        return defaultSettings;
    }

    public static void SetSettings(Settings newSettings)
    {
        settings = newSettings;
        EventBus.Publish(new SettingsChangedEvent(settings));

        SaveManager.Save("settings", settings);
    }

    Settings FixSettings(Settings newSettings)
    {
        return settings;
    }
}
