/*
 * Rotates towards a given aim direction. Only works for 2D.
 * @ Max Perraut '20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WSoft.Math
{
    [System.Serializable]
    public class AimEvent2D : UnityEvent<Vector2> { }

    public class AimPivot2D : MonoBehaviour
    {
        [Tooltip("An event triggered when the transform changes")]
        public AimEvent2D OnAim;

        /// <summary>
        /// Aim the transform towards the direction. Checks if the vector is 0.
        /// </summary>
        /// <param name="aimVec">The direction to aim towards, relative to the right vector</param>
        public void AimToward(Vector2 aimVec)
        {
            if (!Mathf.Approximately(aimVec.sqrMagnitude, 0))
            {
                OnAim.Invoke(aimVec);
                transform.right = aimVec;
            }
        }
    }
}
