using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDisplayTrigger : MonoBehaviour
{
    [TextArea]
    public string text;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            TutorialDisplay.instance.SetTutorialActive(text);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            TutorialDisplay.instance.SetTutorialInactive();
        }
    }
}
