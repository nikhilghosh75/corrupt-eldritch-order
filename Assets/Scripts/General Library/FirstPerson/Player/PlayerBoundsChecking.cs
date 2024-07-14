/*
 * A script which checks if a players is within the bounds of a level
 * Written by Natasha Badami '20, Nigel Charleston '21, Nikhil Ghosh '24
 * 
 * This code may have aspects/assumptions that were specific to its original project. 
 * I would recommend using it as a reference (when implementing a new script), rather than purely copying it and pasting it
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WSoft.FirstPerson
{
    public class PlayerBoundsChecking : MonoBehaviour
    {
        static PlayerBoundsChecking instance;

        public UnityEvent onOutOfBounds;

        // This transform's y-value is used as the y-reset threshold
        Transform outOfBoundsThreshold;
        Vector3 lastSpawnLocation;

        // Used after falling into an infinite pit
        bool resetPositionEndOfFrame;

        void Start()
        {
            if (!instance) instance = this;
            else Destroy(gameObject);

            // Compute the out of bounds position only if an out of bounds threshold GameObject exists
            GameObject foundThreshold = GameObject.Find("OutOfBoundsThreshold");
            if (foundThreshold != null)
            {
                outOfBoundsThreshold = foundThreshold.transform;
            }

            // First checkpoint is always the start location
            lastSpawnLocation = transform.position;
        }

        // Needs to be in late update to bypass Unity character controller's nonsense
        void LateUpdate()
        {
            // If we fell off the map or fell in an infinite pit this frame, reset to last checkpoint
            if (outOfBoundsThreshold != null)
            {
                if (transform.position.y < outOfBoundsThreshold.position.y)
                {
                    resetPositionEndOfFrame = true;
                }
            }

            if (resetPositionEndOfFrame)
            {
                onOutOfBounds?.Invoke(); // use resetpositionendofframe if we want different behavior on infinite pit vs out of bounds
                ResetToLastCheckPoint();
            }
        }

        public static void SetLastCheckpoint(GameObject go, bool setResetPositionEndOfFrame = false)
        {
            instance.lastSpawnLocation = go.transform.position;
            instance.resetPositionEndOfFrame = setResetPositionEndOfFrame;
        }

        public static void ResetToLastCheckPoint()
        {
            instance.transform.position = instance.lastSpawnLocation;
            instance.resetPositionEndOfFrame = false;
        }
    }
}