using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventBus.Publish(new GameCompleteEvent());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
