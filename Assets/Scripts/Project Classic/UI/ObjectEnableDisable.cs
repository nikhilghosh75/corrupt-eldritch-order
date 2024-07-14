using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildEnableDisable : MonoBehaviour
{
    public List<GameObject> objectsToEnable = new List<GameObject>();
    public List<GameObject> objectsToDisable = new List<GameObject>();

    void Start()
    {
        foreach (GameObject o in objectsToDisable)
        {
            o.SetActive(false);
        }

        foreach (GameObject o in objectsToEnable)
        {
            o.SetActive(true);
        }
    }
}
