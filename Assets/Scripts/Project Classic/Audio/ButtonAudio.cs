using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WSoft.Audio;

public class ButtonAudio : MonoBehaviour
{
    public AudioEvent placeholderHover;
    public AudioEvent placeholderSelect;

    public void ButtonHover()
    {
        placeholderHover.PlayAudio(gameObject);
    }

    public void ButtonClick()
    {
        placeholderSelect.PlayAudio(gameObject);
    }
}
