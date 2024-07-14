using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetTextToPlayerClassName : MonoBehaviour
{
    public TextMeshProUGUI classText;
    void Start()
    {
        RunManager runManagerScript = FindAnyObjectByType<RunManager>();
        classText.text = runManagerScript.selectedClassName;
    }
}
