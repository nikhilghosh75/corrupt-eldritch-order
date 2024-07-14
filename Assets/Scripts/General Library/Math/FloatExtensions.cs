using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * An extension class that extends float.
 * For more information, see https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
 */

namespace WSoft.Math
{
    public static class FloatExtensions
    {
        /// <summary>
        /// Exponentially decays to the target value using Time.deltaTime
        /// </summary>
        public static float Fallout(this float origin, float target, float decayRate)
        {
            return Mathf.Lerp(origin, target, 1 - Mathf.Exp(-decayRate * Time.deltaTime));
        }

        /// <summary>
        /// Exponentially decays to the target value using Time.unscaledDeltaTime
        /// </summary>
        public static float FalloutUnscaled(this float origin, float target, float decayRate)
        {
            return Mathf.Lerp(origin, target, 1 - Mathf.Exp(-decayRate * Time.unscaledDeltaTime));
        }

        /// <summary>
        /// Returns the percentage of the total
        /// For instance, 5f.PercentOf(40) = 12.5
        /// </summary>
        public static float PercentOf(this float value, float total)
        {
            return (value / total) * 100f;
        }
    }
}