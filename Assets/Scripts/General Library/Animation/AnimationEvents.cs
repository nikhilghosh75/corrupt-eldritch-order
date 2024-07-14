/*
 * Allows for multiple events to be added to an object in an array and later called through other events,
 * Intended to hook into Animation Events
 * @ George Castle '22
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WSoft.Animation
{
    public class AnimationEvents : MonoBehaviour
    {
        // A struct to wrap the Animation Event in
        [System.Serializable]
        public struct EventInvoker
        {
            public UnityEvent myEvent;
            public string name;
            public void InvokeEvent()
            {
                myEvent.Invoke();
            }
        }

        [Tooltip("The List of Event Invokers")]
        public List<EventInvoker> eventInvokers = new List<EventInvoker>();

        /// <summary>
        /// Invoke the events by their name.
        /// Note that this function is not as performant as InvokeEventByIndex
        /// </summary>
        /// <param name="name">The name of the event</param>
        public void InvokeEventByName(string name)
        {
            for (int i = 0; i < eventInvokers.Count; i++)
            {
                if (eventInvokers[i].name == name)
                {
                    eventInvokers[i].InvokeEvent();
                    return;
                }
            }
        }

        /// <summary>
        /// Invoke the events by their index in the EventInvokers list
        /// </summary>
        /// <param name="index">The index of the event in the list</param>
        public void InvokeEventByIndex(int index)
        {
            eventInvokers[index].InvokeEvent();
        }
    }
}
