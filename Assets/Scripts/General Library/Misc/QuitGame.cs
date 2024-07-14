using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This should be fairly self explanatory
 * Useful for Unity Events
 * Written by Natasha Badami '20
 */

namespace WSoft
{
    public class QuitGame : MonoBehaviour
    {
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}