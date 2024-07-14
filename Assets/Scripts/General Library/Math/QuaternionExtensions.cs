using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * An extension class that extends Quaternion.
 * For more information, see https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
 */

namespace WSoft.Math
{
    public static class QuaternionExtensions
    {
        /// <summary>
        /// Exponentially decays to the target value using Time.deltaTime
        /// </summary>
        public static Quaternion Fallout(this Quaternion origin, Quaternion target, float decayRate)
        {
            return Quaternion.Slerp(origin, target, 1 - Mathf.Exp(-decayRate * Time.deltaTime));
        }

        /// <summary>
        /// Exponentially decays to the target value using Time.unscaledDeltaTime
        /// </summary>
        public static Quaternion FalloutUnscaled(this Quaternion origin, Quaternion target, float decayRate)
        {
            return Quaternion.Slerp(origin, target, 1 - Mathf.Exp(-decayRate * Time.unscaledDeltaTime));
        }
    }
}