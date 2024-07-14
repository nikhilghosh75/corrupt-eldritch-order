/* 
 * Destroy gameobject when its renderer is no longer visible
 * @ Max Perraut '20
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Camera
{
    public class DestroyOutOfSight : MonoBehaviour
    {
        [Tooltip("A renderer that displays the object.")]
        public Renderer view;

        [Tooltip("How long after becoming invisible should this be destroyed?")]
        public float delay = 0;

        private float lastTimeVisible = 0;

        /// <summary>
        /// Gets all of the renderers on the object again, in the event that they change.
        /// </summary>
        public void ResetRenderers()
        {
            //default view to first one found on object
            view = GetComponentInChildren<Renderer>();
        }

        public void Update()
        {
            if (view != null && view.isVisible)
            {
                lastTimeVisible = Time.time;
            }
            else if (Time.time > lastTimeVisible + delay)
            {
                Destroy(gameObject);
            }
        }
    }
}