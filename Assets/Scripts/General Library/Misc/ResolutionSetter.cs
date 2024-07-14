using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft
{
    public class ResolutionSetter : MonoBehaviour
    {
        public int width;
        public int height;

        // Awake is called before Start
        void Awake()
        {
            Screen.SetResolution(width, height, Screen.fullScreen);
        }
    }
}