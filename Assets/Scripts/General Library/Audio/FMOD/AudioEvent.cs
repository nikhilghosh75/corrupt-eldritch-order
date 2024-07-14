using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A script that emmulates Audio Events with FMOD functionality. If FMOD is not being used, this script won't be compiled
 * 
 * Scriptable objects specialized for audio events. allows audio team work to be
 * completely decoupled from all other work, minimizing risk of merge conflicts during the workflow
 * Written by Bradley Gurwin '20, Nikhil Ghosh '24
 */

#if WSOFT_FMOD

namespace WSoft.Audio.FMOD
{
    public class AudioEvent
    {
        [FMODUnity.EventRef]
        public string clip = "";

        public void PlaySound(GameObject caller)
        {
            FMODUnity.RuntimeManager.PlayOneShot(clip, caller.transform.position);
        }
    }
}

#endif