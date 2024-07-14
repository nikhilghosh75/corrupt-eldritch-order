using UnityEngine;
using UnityEngine.Audio;

/*
 * A script that emulates an AudioParam with Unity Audio Mixer.
 * 
 * Audio Params are designed to control the volume of a particular group of audio clips.
 * Written by Nikhil Ghosh '24
 */

namespace WSoft.Audio.Default
{ 
    [System.Serializable]
    public class AudioParam
    {
        public AudioMixer mixer;
        public string settingName;

        public void Set(float newValue)
        {
            mixer.SetFloat(settingName, newValue);
        }
    }
}