using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetImageToClassPortrait : MonoBehaviour
{
    public Image classPortrait;
    public Sprite defaultIcon, ninjaIcon, wizardIcon, tankIcon;

    void Start()
    {
        string className = RunManager.Instance.selectedClassName;

        if (className == "Default")
        {
            classPortrait.sprite = defaultIcon;
        }
        else if (className == "Ninja")
        {
            classPortrait.sprite = ninjaIcon;
        }
        else if (className == "Wizard")
        {
            classPortrait.sprite = wizardIcon;
        }
        else if (className == "Tank")
        {
            classPortrait.sprite = tankIcon;
        }
    }
}
