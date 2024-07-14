using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialDisplay : MonoBehaviour
{
    public TMP_Text text;

    public bool isAvailibleOnStart;

    public static TutorialDisplay instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if (!isAvailibleOnStart)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetTutorialActive(string tutorialText)
    {
        gameObject.SetActive(true);
        text.text = tutorialText;
    }

    public void SetTutorialInactive()
    {
        gameObject.SetActive(false);
    }
}
