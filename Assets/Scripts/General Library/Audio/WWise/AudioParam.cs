using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A script that emulates an AudioParam with WWise RTPCs
 * 
 * Audio Params are designed to control the volume of a particular group of audio clips.
 * Written by Nikhil Ghosh '24
 */

#if WSOFT_WWISE

namespace WSoft.Audio.WWise
{
    [System.Serializable]
    public class AudioParam
    {
        public string rtpcName;

        public void Set(float newValue)
        {
            AkSoundEngine.SetRTPCValue(rtpcName, newValue);
        }
    }

}

#endif