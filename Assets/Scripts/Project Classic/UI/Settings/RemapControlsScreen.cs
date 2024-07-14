using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RemapControlsScreen : MonoBehaviour
{
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        EventBus.Subscribe<RebindControlStartEvent>(OnRemappingStart);
        EventBus.Subscribe<RebindControlEndEvent>(OnRemappingEnd);
    }

    void OnRemappingStart(RebindControlStartEvent e)
    {
        gameObject.SetActive(true);
        text.text = e.actionName;
    }

    void OnRemappingEnd(RebindControlEndEvent e)
    {
        gameObject.SetActive(false);
    }
}
