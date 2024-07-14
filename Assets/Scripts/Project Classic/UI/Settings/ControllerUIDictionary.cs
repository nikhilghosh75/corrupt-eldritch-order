using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ControllerUIDictionary : MonoBehaviour
{

    public string[] controllerNames;
    public Sprite[] controllerSprites;

    public Dictionary<string, Sprite> controllerUIDictionary = new Dictionary<string, Sprite>();

    public static ControllerUIDictionary Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
            Instance = this;

            for (int i = 0; i < controllerNames.Length; i++)
            {
                controllerUIDictionary[controllerNames[i]] = controllerSprites[i];
            }

            //RemappableControlButton[] buttons = FindObjectsOfType<RemappableControlButton>();
            //for (int i = 0; i < buttons.Length; i++)
            //{
            //    buttons[i].UpdateDisplay();
            //}
        }
    }
}
