using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Constrains the position of the game object to it's original position
 * Written by Nikhil Ghosh '24
 */ 

namespace WSoft.Animation
{
    public enum Axis
    {
        None,
        X,
        Y,
        XY,
        Z,
        XZ,
        YZ,
        XYZ
    }


    public class ConstrainPosition : MonoBehaviour
    {
        [Tooltip("The axes along which the position should be constrain")]
        public Axis axesToConstrain;

        public bool active = true;

        Vector3 original; // The original rotation

        // Start is called before the first frame update
        void Start()
        {
            original = transform.localPosition;
        }


        void LateUpdate()
        {
            if (!active)
            {
                return;
            }

            Vector3 current = transform.localPosition;
            Vector3 final = Vector3.zero;

            switch (axesToConstrain)
            {
                case Axis.None:
                    final = current;
                    break;
                case Axis.X:
                    final = new Vector3(original.x, current.y, current.z);
                    break;
                case Axis.Y:
                    final = new Vector3(current.x, original.y, current.z);
                    break;
                case Axis.Z:
                    final = new Vector3(current.x, current.y, original.z);
                    break;
                case Axis.XY:
                    final = new Vector3(original.x, original.y, current.z);
                    break;
                case Axis.XZ:
                    final = new Vector3(original.x, current.y, original.z);
                    break;
                case Axis.YZ:
                    final = new Vector3(current.x, original.y, original.z);
                    break;
                case Axis.XYZ:
                    final = original;
                    break;
            }

            transform.localPosition = final;
        }
    }
}