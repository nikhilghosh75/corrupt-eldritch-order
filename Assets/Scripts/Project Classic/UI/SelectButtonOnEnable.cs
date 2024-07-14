using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButtonOnEnable : MonoBehaviour
{
    public Button buttonToSelect;

    private void OnEnable()
    {
        buttonToSelect.Select();
    }
}
