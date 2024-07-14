/*
    Rotates an object towards a tagged target
    Written by Natasha Badami '20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft
{
    public class RotateToTarget : MonoBehaviour
    {
        // IF USING TARGET WITH TAG
        [SerializeField] bool rotateToTag;
        [SerializeField] string tagToRotateTo;
        GameObject targetWithTag { get; set; }

        void Start()
        {
            if (rotateToTag)
            {
                targetWithTag = GameObject.FindGameObjectWithTag(tagToRotateTo);
            }
        }

        public void FaceTargetWithTag()
        {
            transform.LookAt(targetWithTag.transform);
        }

        public void FaceTargetWithVector3(Vector3 targetVec)
        {
            transform.LookAt(targetVec);
        }
    }
}