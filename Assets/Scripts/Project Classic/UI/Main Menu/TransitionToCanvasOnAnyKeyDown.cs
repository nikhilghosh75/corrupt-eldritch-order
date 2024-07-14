using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToCanvasOnAnyKeyDown : MonoBehaviour
{
    public string canvasName;

    public void OnSubmit()
    {
        EventBus.Publish<MainMenuTransitionEvent>(new MainMenuTransitionEvent(canvasName));
    }
}
