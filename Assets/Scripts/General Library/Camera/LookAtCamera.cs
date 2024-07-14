/* 
 * Makes sprites face the player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Camera
{
    public class LookAtCamera : MonoBehaviour
    {
        // A container for three bools which specify which axis or axes 
        // of the sprite to point toward the player
        [System.Serializable]
        public class RotationAxis
        {
            public bool x;
            public bool y = true;
            public bool z;
        }

        [Tooltip("A constant angle added to each axis of rotation. Used to correct sprites that do not face straight forward.")]
        [SerializeField] Vector3 offset;

        [Tooltip("Specifies which axis of the sprite to point toward the player.")]
        [SerializeField] RotationAxis FollowAxis;

        void Update()
        {
            transform.rotation = Quaternion.LookRotation(UnityEngine.Camera.main.transform.forward);

            if (!FollowAxis.x)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
            }
            if (!FollowAxis.y)
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, 0f, transform.rotation.eulerAngles.z));
            }
            if (!FollowAxis.z)
            {
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f));
            }
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x + offset.x, transform.rotation.eulerAngles.y + offset.y, transform.rotation.eulerAngles.z + offset.z));
        }
    }
}