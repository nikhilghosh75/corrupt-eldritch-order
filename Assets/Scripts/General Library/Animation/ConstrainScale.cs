using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Constrains the scale of the game object to it's original scale
 * Written by Nikhil Ghosh '24
 */

namespace WSoft.Animation
{
    public class ConstrainScale : MonoBehaviour
    {
        [Tooltip("The axes along which the position should be constrain")]
        public Axis axesToConstrain;

        public bool active = true;

        Vector3 original; // The original rotation

        // Start is called before the first frame update
        void Start()
        {
            original = transform.localScale;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (!active)
            {
                return;
            }

            Vector3 currentScale = transform.localScale;
            Vector3 finalScale = new Vector3(0, 0, 0);

            switch (axesToConstrain)
            {
                case Axis.None:
                    finalScale = currentScale;
                    break;
                case Axis.X:
                    finalScale = new Vector3(original.x, currentScale.y, currentScale.z);
                    break;
                case Axis.Y:
                    finalScale = new Vector3(currentScale.x, original.y, currentScale.z);
                    break;
                case Axis.Z:
                    finalScale = new Vector3(currentScale.x, currentScale.y, original.z);
                    break;
                case Axis.XY:
                    finalScale = new Vector3(original.x, original.y, currentScale.z);
                    break;
                case Axis.XZ:
                    finalScale = new Vector3(original.x, currentScale.y, original.z);
                    break;
                case Axis.YZ:
                    finalScale = new Vector3(currentScale.x, original.y, original.z);
                    break;
                case Axis.XYZ:
                    finalScale = original;
                    break;
            }

            transform.localScale = finalScale;
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
