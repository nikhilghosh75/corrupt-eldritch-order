/*
 * This script detects hitscan and returns hit object or null.
 * It can be used to hook up raycast events and return the object
 * that triggered the hitscan. On a miss it returns null.
 * @ Evan Brisita '18
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WSoft.Combat
{
    public class HitscanEvents : MonoBehaviour
    {
        [System.Serializable]
        public class HitscanEvent : UnityEngine.Events.UnityEvent<GameObject> { }

        public HitscanEvent OnHitscanHit;
        public HitscanEvent OnHitscanMiss;

        [Header("Options")]
        [Tooltip("Filter out what triggers a raycast")]
        public LayerMask detectionMask;
        [Tooltip("Set the origin of the raycast and the forward vector")]
        public Transform raycastOrigin;
        [Tooltip("If true then raycast use infinite length")]
        public bool hasInfiniteRange;
        public float raycastRange;

        private void Awake()
        {
            // Query for infinite raycast range
            raycastRange = hasInfiniteRange ? Mathf.Infinity : raycastRange;
        }

        /*
         * Hitscans using the forward vector of raycastOrigin.
         * Fires off Hitscan events
         */
        public RaycastHit DetectHitscan()
        {
            return DetectHitscan(raycastOrigin.forward);
        }

        /*
         * Hitscans using custom direction.
         * Fires off Hitscan events
         */
        public RaycastHit DetectHitscan(Vector3 direction)
        {
            RaycastHit hit;

            // Send out raycast and query for hit detection
            // Raycast hit
            if (Physics.Raycast(raycastOrigin.position, direction,
                out hit, raycastRange, detectionMask))
            {
                // Invoke event and pass hitscanned gameobject
                OnHitscanHit?.Invoke(hit.collider.gameObject);
            }
            // Raycast missed
            else
            {
                // Invoke event and pass null
                OnHitscanMiss?.Invoke(null);
            }
            return hit;
        }
    }
}