/* 
 * Set animator on object to only be enabled when any renderers are visible
 * @ Max Perraut '20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Animation
{
    [RequireComponent(typeof(Animator))]
    public class AnimateWhenVisible : MonoBehaviour
    {
        public Renderer[] renderers;

        private Animator anim;

        /// <summary>
        /// Gets all of the renderers on the object again, in the event that they change.
        /// </summary>
        public void ResetRenderers()
        {
            // find all renderers when attached to gameobject
            renderers = GetComponentsInChildren<Renderer>();
        }

        void Awake()
        {
            anim = GetComponent<Animator>();
            renderers = GetComponentsInChildren<Renderer>();
        }

        // Update is called once per frame
        void Update()
        {
            // if any renderer is visible, enable animation
            foreach (Renderer r in renderers)
            {
                if (r.isVisible)
                {
                    anim.enabled = true;
                    return;
                }
            }

            // if no renderers are visible, disable animation
            anim.enabled = false;
        }
    }
}

