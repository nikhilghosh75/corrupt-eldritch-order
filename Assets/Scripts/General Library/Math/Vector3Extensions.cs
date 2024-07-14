using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * An extension class that extends Vector3.
 * For more information, see https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
 */

namespace WSoft.Math
{
    public static class Vector3Extensions
    {
        /// <summary>
        /// Exponentially decays to the target value using Time.deltaTime
        /// </summary>
        public static Vector3 Fallout(this Vector3 origin, Vector3 target, float decayRate)
        {
            return Vector3.Lerp(origin, target, 1 - Mathf.Exp(-decayRate * Time.deltaTime));
        }

        /// <summary>
        /// Exponentially decays to the target value using Time.unscaledDeltaTime
        /// </summary>
        public static Vector3 FalloutUnscaled(this Vector3 origin, Vector3 target, float decayRate)
        {
            return Vector3.Lerp(origin, target, 1 - Mathf.Exp(-decayRate * Time.unscaledDeltaTime));
        }
    }
}