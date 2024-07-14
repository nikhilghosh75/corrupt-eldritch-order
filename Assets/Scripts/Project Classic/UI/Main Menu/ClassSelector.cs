using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClassSelector : MonoBehaviour
{
    public float timeDisabled = 0.5f;
    public EventSystem eventSystem;
    public GameObject leftButton, centerButton, rightButton;

    private bool transitioning = false;

    // Update is called once per frame
    void Update()
    {
        if (transitioning)
            return;

        GameObject selectedObject = eventSystem.currentSelectedGameObject;
        if (selectedObject == leftButton || selectedObject == rightButton)
        {
            StartCoroutine(SideButtonSelected());
        }
    }

    private IEnumerator SideButtonSelected()
    {
        transitioning = true;
        // disable the event system 
        eventSystem.enabled = false;
        yield return new WaitForSeconds(timeDisabled);
        // enable the event system 
        eventSystem.enabled = true;
        eventSystem.SetSelectedGameObject(centerButton);
        transitioning = false;
    }
}
