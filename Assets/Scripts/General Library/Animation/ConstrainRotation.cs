using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Constrains the rotation of the game object to it's original rotation
 * Written by Nikhil Ghosh '24
 */

namespace WSoft.Animation
{
    public class ConstrainRotation : MonoBehaviour
    {
        [Tooltip("The axes along which the position should be constrain")]
        public Axis axesToConstrain;

        public bool active = true;

        Quaternion originalRotation; // The original rotation

        // Start is called before the first frame update
        void Start()
        {
            originalRotation = transform.rotation;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            Vector3 original = originalRotation.eulerAngles;
            Vector3 euler = transform.rotation.eulerAngles;
            Vector3 finalRotation = new Vector3(0, 0, 0);

            switch (axesToConstrain)
            {
                case Axis.None:
                    finalRotation = euler;
                    break;
                case Axis.X:
                    finalRotation = new Vector3(original.x, euler.y, euler.z);
                    break;
                case Axis.Y:
                    finalRotation = new Vector3(euler.x, original.y, euler.z);
                    break;
                case Axis.Z:
                    finalRotation = new Vector3(euler.x, euler.y, original.z);
                    break;
                case Axis.XY:
                    finalRotation = new Vector3(original.x, original.y, euler.z);
                    break;
                case Axis.XZ:
                    finalRotation = new Vector3(original.x, euler.y, original.z);
                    break;
                case Axis.YZ:
                    finalRotation = new Vector3(euler.x, original.y, original.z);
                    break;
                case Axis.XYZ:
                    finalRotation = original;
                    break;
            }

            transform.rotation = Quaternion.Euler(finalRotation);
        }

        public void Activate()
        {
            active = true;
        }

        public void Deactivate()
        {
            active = false;
        }
    }
}
