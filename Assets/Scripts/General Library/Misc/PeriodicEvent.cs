using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * Calls a UnityEvent on a given internal.
 * Written by Natasha Badami '20
 */

namespace WSoft
{
    public class PeriodicEvent : MonoBehaviour
    {
        public UnityEvent MyEvent;
        [SerializeField] float timer;
        public bool dontStop { get; set; }

        public void StartInvoking()
        {
            InvokeRepeating("CallEvent", 0f, timer);
        }

        void CallEvent()
        {
            MyEvent.Invoke();
        }

        public void HaltInvoking()
        {
            if (!dontStop)
            {
                CancelInvoke();
            }
        }
    }

}