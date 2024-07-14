using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * Written by Natasha Badami '20, Alex Czarnecki '22
 */

namespace WSoft
{
    public class StartEvent : MonoBehaviour
    {
        [Tooltip("Event to invoke on start")]
        public UnityEvent OnStartEvent;

        void Start()
        {
            OnStartEvent.Invoke();
        }
    }
}