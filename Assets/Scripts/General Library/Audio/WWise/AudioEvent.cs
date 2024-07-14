using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A script that emmulates Audio Events with WWise Events. If WWise is not being used, this script won't be compiled
 * 
 * Scriptable objects specialized for audio events. allows audio team work to be
 * completely decoupled from all other work, minimizing risk of merge conflicts during the workflow
 * Written by Nico Williams '20, Nikhil Ghosh '24
 */

#if WSOFT_WWISE

namespace WSoft.Audio.WWise
{
    [System.Serializable]
    public class AudioEvent
    {
        public AK.Wwise.Event wwiseEvent;

        public void PlaySound(GameObject caller)
        {
            if (wwiseEvent.IsValid())
            {
                wwiseEvent.Post(caller);
            }
            else
            {
                Debug.LogWarning(wwiseEvent.Name + " is not a valid WWise event");
            }
        }
    }
}

#endif