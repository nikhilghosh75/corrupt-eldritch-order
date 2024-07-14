using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartFade : MonoBehaviour
{
    [SerializeField] GameObject fadePanel;

    public static LevelStartFade Instance { get; private set; }

    void Awake()
    {
        Instance = this;

        fadePanel.SetActive(true);
        fadePanel.GetComponent<Animator>().SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        fadePanel.GetComponent<Animator>().SetTrigger("FadeOut");
    }
}
