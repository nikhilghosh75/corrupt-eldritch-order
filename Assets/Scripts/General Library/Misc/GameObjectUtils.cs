using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft
{
    public static class GameObjectUtils
    {
        public static string GetPath(this Transform current, char separator = '/')
        {
            if (current.parent == null)
                return separator + current.name;
            return current.parent.GetPath() + separator + current.name;
        }
    }
}
