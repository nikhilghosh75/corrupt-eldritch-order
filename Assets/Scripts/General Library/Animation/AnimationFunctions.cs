using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Animation Functions is a class meant to carry a bunch of static functions that may need to be called by any class relating to animators and animation.
 * Note that since all functions are static, this is to be called like AnimatorFunctions.HasParameter(...)
 * Written by Nikhil Ghosh '24
 */

namespace WSoft.Animation
{
    public class AnimationFunctions : MonoBehaviour
    {
        /// <summary>
        /// Returns true if an animator has a specific parameter, false if it doesn't have that parameter
        /// </summary>
        public static bool HasParameter(Animator animator, string parameterName)
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.name == parameterName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if an animator has all the parameters in the specified list, returns false if at least one is missing
        /// </summary>
        public static bool HasParameters(Animator animator, string[] parameterNames)
        {
            for (int i = 0; i < parameterNames.Length; i++)
            {
                if (!HasParameter(animator, parameterNames[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns an array of all the parameters that are missing in an animator from an array of parameters
        /// </summary>
        public static string[] ParametersMissing(Animator animator, string[] parameterNames)
        {
            string[] missingArray = new string[animator.parameterCount];
            int missingLength = 0;
            for (int i = 0; i < animator.parameterCount; i++)
            {
                if (!HasParameter(animator, parameterNames[i]))
                {
                    missingArray[missingLength] = parameterNames[i];
                    missingLength++;
                }
            }

            if (missingLength == 0)
            {
                return null;
            }

            string[] returnArray = new string[missingLength];
            for (int i = 0; i < missingLength; i++)
            {
                returnArray[i] = missingArray[i];
            }
            return missingArray;
        }

        /// <summary>
        /// Returns an array of all the parameters in an animator
        /// </summary>
        public static string[] AnimatorParameters(Animator animator)
        {
            string[] parameterArray = new string[animator.parameterCount];
            for (int i = 0; i < animator.parameterCount; i++)
            {
                parameterArray[i] = animator.parameters[i].name;
            }
            return parameterArray;
        }
    }

}