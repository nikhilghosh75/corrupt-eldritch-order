using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Keeps graphics settings saved between scenes so the options menu can be up to date
 * 
 * @ Bri Epstein '22, Nikhil Ghosh '24
 */

public class GraphicsSettingsManager : MonoBehaviour
{
    public const string RESOLUTION_KEY = "Resolution";
    public const string QUALITY_KEY = "Quality";
    public const string FULLSCREEN_KEY = "Fullscreen";

    public int resolutionOption
    {
        set
        {
            _resolutionOption = value;
            PlayerPrefs.SetInt(RESOLUTION_KEY, value);
        }
        get { return _resolutionOption; }
    }
    public int qualityOption
    {
        set
        {
            _qualityOption = value;
            PlayerPrefs.SetInt(QUALITY_KEY, value);
        }
        get { return _qualityOption; }
    }
    public bool fullscreenOption
    {
        set
        {
            _fullscreenOption = value;
            PlayerPrefs.SetInt(FULLSCREEN_KEY, Convert.ToInt32(value));
            Screen.fullScreen = _fullscreenOption;
        }
        get { return _fullscreenOption; }
    }

    private int _resolutionOption;
    private int _qualityOption;
    private bool _fullscreenOption;

    private static GraphicsSettingsManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        SetDefaults();
        DontDestroyOnLoad(this.gameObject);
        Debug.Log("_resolutionOption: " + _resolutionOption + ", _qualityOption: " + _qualityOption + ", _fullscreenOption: " + _fullscreenOption);
    }

    public static GraphicsSettingsManager Get() { return instance; }

    public void SetDefaults()
    {
        if(!PlayerPrefs.HasKey(RESOLUTION_KEY))
        {
            DefaultToHighestRes();
            _resolutionOption = -1;
        }
        else
        {
            _resolutionOption = PlayerPrefs.GetInt(RESOLUTION_KEY);
        }

        _qualityOption = PlayerPrefs.GetInt(QUALITY_KEY, 6);
        _fullscreenOption = Convert.ToBoolean(PlayerPrefs.GetInt(FULLSCREEN_KEY, 1));
        Debug.Log("_resolutionOption: " + _resolutionOption + ", _qualityOption: " + _qualityOption + ", _fullscreenOption: " + _fullscreenOption);
    }

    //find highest res less than or equal to default/native res
    public void DefaultToHighestRes()
    {
        Resolution[] resolutions = FindAllowedResolutions();
        resolutionOption = resolutions.Length - 1;

        ForceResChange(resolutions[resolutionOption].width, resolutions[resolutionOption].height);
    }

    void ForceResChange(int width, int height)
    {
        Screen.SetResolution(width, height, Screen.fullScreenMode);
    }

    public Resolution[] FindAllowedResolutions()
    {
        Resolution[] allMonitorRes = Screen.resolutions;
        List<Resolution> allowedResolutions = new List<Resolution>();

        float aspectRatio = (float)allMonitorRes[allMonitorRes.Length - 1].width / (float)allMonitorRes[allMonitorRes.Length - 1].height;
        float aspectRatio16x9 = 16.0f / 9.0f;

        //find first resolution with correct aspect ratio
        int first = 0;
        float thisRatio = (float)allMonitorRes[first].width / (float)allMonitorRes[first].height;
        while (first < allMonitorRes.Length && (Mathf.Abs(thisRatio - aspectRatio) > 0.001f && Mathf.Abs(thisRatio - aspectRatio16x9) > 0.001f))
        {
            first++;
            if (first >= allMonitorRes.Length) break;
            thisRatio = (float)allMonitorRes[first].width / (float)allMonitorRes[first].height;
        }

        //store widths/heights to avoid  dupes
        int lastWidth = allMonitorRes[first].width;
        int lastHeight = allMonitorRes[first].height;
        allowedResolutions.Add(allMonitorRes[first]);

        int i = 1; // keep track of number of unique resolutions
        foreach (Resolution r in allMonitorRes)
        {
            //prevent duplicate resolutions
            float thisAspectRatio = (float)r.width / (float)r.height;
            if ((r.width == lastWidth && r.height == lastHeight) || (Mathf.Abs(thisAspectRatio - aspectRatio) > 0.001f && Mathf.Abs(thisAspectRatio - aspectRatio16x9) > 0.001f)) continue;
            lastWidth = r.width;
            lastHeight = r.height;
            allowedResolutions.Add(r);
            i++;
        }
        return allowedResolutions.ToArray();
    }
}
