using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A script that emulates an AudioParam with FMOD VCAs. If FMOD is not being used, this script won't be compiled. 
 * 
 * Audio Params are designed to control the volume of a particular group of audio clips.
 * Written by Nikhil Ghosh '24
 */

#if WSOFT_FMOD

namespace WSoft.Audio.FMOD
{
    [System.Serializable]
    public class AudioParam
    {
        public string vcaPath;

        FMOD.Studio.VCA vca = null;

        public void Set(float newValue)
        {
            if(vca == null)
            {
                vca = FMODUnity.RuntimeManager.GetVCA(vcaPath);
            }

            vca.setVolume(newValue);
        }
    }
}

#endif
