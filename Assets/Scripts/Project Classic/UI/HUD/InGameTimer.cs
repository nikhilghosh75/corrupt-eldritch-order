using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameTimer : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        float timeSinceStart = Time.time - startTime;

        string minutes = Mathf.Floor(timeSinceStart / 60).ToString("00");
        string seconds = (timeSinceStart % 60).ToString("00");

        timeText.text = $"{minutes}:{seconds}";
    }
}
