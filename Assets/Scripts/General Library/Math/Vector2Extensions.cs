using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * An extension class that extends Vector2.
 * For more information, see https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
 */

namespace WSoft.Math
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Exponentially decays to the target value using Time.deltaTime
        /// </summary>
        public static Vector2 Fallout(this Vector2 origin, Vector2 target, float decayRate)
        {
            return Vector2.Lerp(origin, target, 1 - Mathf.Exp(-decayRate * Time.deltaTime));
        }

        /// <summary>
        /// Exponentially decays to the target value using Time.unscaledDeltaTime
        /// </summary>
        public static Vector2 FalloutUnscaled(this Vector2 origin, Vector2 target, float decayRate)
        {
            return Vector2.Lerp(origin, target, 1 - Mathf.Exp(-decayRate * Time.unscaledDeltaTime));
        }
    }
}