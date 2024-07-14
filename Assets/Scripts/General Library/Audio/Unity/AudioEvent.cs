using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/*
 * A script that emmulates Audio Events with Unity's AudioClip
 * 
 * Scriptable objects specialized for audio events. allows audio team work to be
 * completely decoupled from all other work, minimizing risk of merge conflicts during the workflow
 * Written by Nikhil Ghosh '24
 */

namespace WSoft.Audio.Default
{
    [System.Serializable]
    public class AudioEvent
    {
        public AudioClip clip;

        public void PlaySound(GameObject caller)
        {
            if(clip == null)
            {
                Debug.LogWarning("Warning: clip is null");
                return;
            }

            AudioSource source = caller.GetComponent<AudioSource>();
            if(source == null)
            {
                source = caller.AddComponent<AudioSource>();
            }

            source.PlayOneShot(clip);
        }
    }
}

