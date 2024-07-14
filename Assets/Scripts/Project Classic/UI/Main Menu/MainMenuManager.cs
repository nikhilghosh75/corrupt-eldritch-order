using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenu;

    public GameObject settingsMenu;

    public GameObject runConfigurationMenu;

    public GameObject titleMenu;

    // Start is called before the first frame update
    void Start()
    {
        settingsMenu.SetActive(false);
        runConfigurationMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void SetSettingsActive(bool active)
    {
        settingsMenu.SetActive(active);
    }

    public void SetRunConfigurationActive(bool active)
    {
        runConfigurationMenu.SetActive(active);
    }
    public void SetMainMenuActive(bool active) 
    {
        mainMenu.SetActive(active);
    }

    public void SetTitleScreenActive(bool active) 
    {
        titleMenu.SetActive(active);
    }
}
