/*
 *  Debounce prevents a function from being invoked within a certain period of time
 *  This prevents callbacks from being called too many times, for example in the event system
 *  where a callback can be fired several times (e.g. Start, Finish) from the same event (e.g. Press).
 * 
 *  This implementation doesn't do any invoking, just provides a boolean check.
 *  @Jacob Shreve
 */

using System.Collections.Generic;
using UnityEngine;

namespace WSoft
{
    public class Debounce
    {
        static Dictionary<string, float> dict = new Dictionary<string, float>();

        // <summary>
        // Test if the given event has been called within the last delay seconds.
        // Uses unscaled time.
        // </summary>
        public static bool Test(string name, float delay)
        {
            if (!dict.ContainsKey(name))
            {
                // Debug.Log("[Debounce] First event: " + name);
                dict.Add(name, Time.unscaledTime + delay);
                return true;
            }

            if (dict[name] < Time.unscaledTime)
            {
                // Debug.Log("[Debounce] Allowing event: " + name);
                dict[name] = Time.unscaledTime + delay;
                return true;
            }

            // Debug.Log("[Debounce] Rejecting event: " + name);
            return false;
        }
    }
}