using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartRunButton : MonoBehaviour
{
    Button button;
    [SerializeField] GameObject fadeObject;
    bool transitioning = false;

    float fadeLength = 2.0f; // If the animation length changes, change this number too.

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(StartFade);
    }

    void StartFade()
    {
        if (!transitioning)
        {
            transitioning = true;
            fadeObject.SetActive(true); // Enable the fade canvas object
            fadeObject.GetComponentInChildren<Animator>().SetTrigger("FadeOut"); // Start the fadeout animation
            Invoke(nameof(StartRun), fadeLength);
        }
    }

    public void StartRun()
    {
        GameManager.Instance.useSeed = false;
        GameManager.Instance.StartRun();
    }
}
