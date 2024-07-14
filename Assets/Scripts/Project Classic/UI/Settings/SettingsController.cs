using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SettingsTab
{
    public string name;
    public GameObject gameObject;
}

public class SettingsController : MonoBehaviour
{
    public List<SettingsTab> tabs;
    public int startingTabIndex;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            if (i == startingTabIndex)
            {
                tabs[i].gameObject.SetActive(true);
            }
            else
            {
                tabs[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenTab(string tabName)
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            if (tabs[i].name == tabName)
            {
                tabs[i].gameObject.SetActive(true);
            }
            else
            {
                tabs[i].gameObject.SetActive(false);
            }
        }
    }
}
