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
    public class AimEvent : UnityEvent<Vector3> { }

    public class AimPivot : MonoBehaviour
    {
        [Tooltip("An event triggered when the transform changes")]
        public AimEvent OnAim;

        /// <summary>
        /// Aim the transform towards the direction. Checks if the vector is 0.
        /// </summary>
        /// <param name="aimVec">The direction to aim towards, relative to the right vector</param>
        public void AimToward(Vector3 aimVec)
        {
            if (!Mathf.Approximately(aimVec.sqrMagnitude, 0))
            {
                OnAim.Invoke(aimVec);
                transform.right = aimVec;
            }
        }
    }
}
