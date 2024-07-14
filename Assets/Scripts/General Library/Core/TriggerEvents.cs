/* Invokes events for trigger messages
 * Each event passes the gameobject of the collider that caused the trigger
 * Written by Max Perraut '20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Core
{
    public class TriggerEvents : MonoBehaviour
    {
        [System.Serializable]
        public class TriggerEvent : UnityEngine.Events.UnityEvent<GameObject> { }

        public TriggerEvent OnTriggerEnterEvent;
        public TriggerEvent OnTriggerStayEvent;
        public TriggerEvent OnTriggerExitEvent;

        [Tooltip("If true, only invoke events for object with tag <triggerTag>.")]
        public bool triggerOnlyForTag = false;
        public string triggerTag = "";

        //Check given object against any filters
        private bool Filter(GameObject checkObj)
        {
            return !triggerOnlyForTag || checkObj.CompareTag(triggerTag);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (Filter(collision.gameObject))
            {
                OnTriggerEnterEvent.Invoke(collision.gameObject);
            }
        }

        private void OnTriggerStay(Collider collision)
        {
            if (Filter(collision.gameObject))
            {
                OnTriggerStayEvent.Invoke(collision.gameObject);
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            if (Filter(collision.gameObject))
            {
                OnTriggerExitEvent.Invoke(collision.gameObject);
            }
        }
    }
}