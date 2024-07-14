/* 
 * Activates child gameobjects after a delay measured in frames.
 * Used for delaying sound bank activation when a scene is loaded
 * @ Max Perraut '20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Audio
{
    public class ActivateChildren : MonoBehaviour
    {
        [Tooltip("The delay between the start of the scene and the activation of the children, in frames")]
        public int frameDelay = 1;

        void Start()
        {
            StartCoroutine(DoDelayedActivation());
        }

        /// <summary>
        /// Delays the activaton until the specified number of frames
        /// </summary>
        private IEnumerator DoDelayedActivation()
        {
            // wait for <frameDelay> frames
            for (int i = 0; i < frameDelay; i++)
            {
                yield return null;
            }

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

}