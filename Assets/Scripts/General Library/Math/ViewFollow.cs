/* 
 * Matches this gameObject's position and rotation values to another's.
 * @ Max Perraut '20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Math
{
    public class ViewFollow : MonoBehaviour
    {
        [Tooltip("The transform to match the position and rotation of")]
        public Transform follow;

        [Tooltip("Set true to use the world transform, false to use local transform")]
        public bool useWorld = false;

        private void LateUpdate()
        {
            if (useWorld)
            {
                transform.position = follow.position;
                transform.rotation = follow.rotation;
            }
            else
            {
                transform.localPosition = follow.localPosition;
                transform.localRotation = follow.localRotation;
            }
        }
    }
}
