using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventBus.Subscribe<UpdateHealthEvent>(OnHealthChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnHealthChanged(UpdateHealthEvent e)
    {
        if (e.health + e.healthDelta <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
