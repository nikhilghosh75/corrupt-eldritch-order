using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A script that plays an audio event on the awake method
 * Written by Amber Renton the Great '22
 */

namespace WSoft.Audio
{
    public class AudioOnAwake : MonoBehaviour
    {
        public AudioEvent audioEvent;

        void Awake()
        {
            audioEvent.PlayAudio(gameObject);
        }
    }
}
