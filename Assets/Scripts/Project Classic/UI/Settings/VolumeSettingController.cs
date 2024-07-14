using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WSoft.Audio.WWise;

public enum VolumeType
{
    Master,
    Music,
    SFX,
    Dialogue,
    Enemies,
    Player,
    UI
}

public class VolumeSettingController : MonoBehaviour
{
    public AudioParam volume;
    public VolumeType type;
    public VolumeSettingController parent;
    public List<VolumeSettingController> children;

    public TMP_Text nameText;
    public TMP_Text valueText;
    public Button increaseButton;
    public Button decreaseButton;
    public Button muteButton;

    [Header("Visuals")]
    public Sprite mutedSprite;
    public Sprite unmutedSprite;

    [Header("Colors")]
    public Color enabledTextColor;
    public Color disabledTextColor;

    int value = 5;
    bool muted = false;

    // Start is called before the first frame update
    void Start()
    {
        value = GetValueFromSettings();

        nameText.color = enabledTextColor;
        valueText.color = enabledTextColor;

        SetValue(value);
    }

    public void SetValue(int _value)
    {
        volume.Set(_value * 10);
        value = _value;

        valueText.text = value.ToString();
        ApplyValueToSettings();
        

        // Removed due to navigation breaking and not being a neccessity
        //increaseButton.interactable = value < 10;
        //decreaseButton.interactable = value > 0;
    }

    public void Increase()
    {
        if (value < 10)
        {
            SetValue(value + 1);
        }
    }

    public void Decrease()
    {
        if (value > 0)
        {
            SetValue(value - 1);
        }
    }

    public void ToggleMute()
    {
        if (muted)
        {
            Unmute();
        }
        else
        {
            Mute();
        }
        muted = !muted;
    }

    public void Mute()
    {
        volume.Set(0);
        increaseButton.interactable = false;
        decreaseButton.interactable = false;

        muteButton.image.sprite = mutedSprite;
        nameText.color = disabledTextColor;
        valueText.color = disabledTextColor;

        foreach (VolumeSettingController child in children)
        {
            child.Mute();
        }
    }

    public void Unmute()
    {
        SetValue(value);
        SetDisplayUnmuted();
    }

    public void SetDisplayUnmuted()
    {
        muteButton.image.sprite = unmutedSprite;
        nameText.color = enabledTextColor;
        valueText.color = enabledTextColor;

        if (parent != null)
        {
            parent.SetDisplayUnmuted();
        }
    }

    int GetValueFromSettings()
    {
        switch(type)
        {
            case VolumeType.Master:
                return SettingsLoader.Settings.masterVolume;
            case VolumeType.Music:
                return SettingsLoader.Settings.musicVolume;
            case VolumeType.SFX:
                return SettingsLoader.Settings.sfxVolume;
            case VolumeType.Dialogue:
                return SettingsLoader.Settings.dialogueVolume;
            case VolumeType.Enemies:
                return SettingsLoader.Settings.enemiesVolume;
            case VolumeType.Player:
                return SettingsLoader.Settings.playerVolume;
            case VolumeType.UI:
                return SettingsLoader.Settings.uiVolume;
        }
        return 5;
    }

    void ApplyValueToSettings()
    {
        Settings settings = SettingsLoader.Settings;

        switch (type)
        {
            case VolumeType.Master:
                settings.masterVolume = value;
                break;
            case VolumeType.Music:
                settings.musicVolume = value;
                break;
            case VolumeType.SFX:
                settings.sfxVolume = value;
                break;
            case VolumeType.Dialogue:
                settings.dialogueVolume = value;
                break;
            case VolumeType.Enemies:
                settings.enemiesVolume = value;
                break;
            case VolumeType.Player:
                settings.playerVolume = value;
                break;
            case VolumeType.UI:
                settings.uiVolume = value;
                break;
        }

        SettingsLoader.SetSettings(settings);
    }
}
