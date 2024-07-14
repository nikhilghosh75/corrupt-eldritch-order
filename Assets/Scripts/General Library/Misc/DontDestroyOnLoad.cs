/*
 * A helper script to prevent destruction of a game object when destroyed
 * Written by Alex Czarnecki '22
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

}