using System;
using UnityEngine;

/*
 * An attribute that specifies that a string refers to a Unity scene
 * I'm not actually sure what namespace this should go under, if anyone has any ideas that would be nice
 * Written by Nikhil Ghosh '24
 */

namespace WSoft
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SceneAttribute : PropertyAttribute
    {

    }
}