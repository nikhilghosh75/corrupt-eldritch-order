using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*
 * A script that augments the usual Select functionality
 * Written by Nikhil Ghosh '24
 */

namespace WSoft.Input
{
    public class SelectOnlyWithGamepad : MonoBehaviour
    {
        public void Select()
        {
            if (Gamepad.current != null)
                GetComponent<Selectable>().Select();
        }
    }
}