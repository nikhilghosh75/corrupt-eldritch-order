/* 
 * Scriptable objects specialized for audio events. allows audio team work to be
 * completely decoupled from all other work, minimizing risk of merge conflicts during the workflow
 * Written by Nikhil Ghosh '24
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WSoft.Audio
{
    [CreateAssetMenu(fileName = "New AudioEvent", menuName = "WSoft/Audio Event")]
    public class AudioEvent : ScriptableObject
    {
        // This is inteded to replicate the behavior of an audio event across different audio backends
        // An audio event stores information about an audio clip into a scriptable object
#if WSOFT_WWISE
        public WWise.AudioEvent audioEvent;
#elif WSOFT_MFOD
        public FMOD.AudioEvent audioEvent;
#else
        public Default.AudioEvent audioEvent;
#endif

        /// <summary>
        /// Plays the sound
        /// </summary>
        /// <param name="caller">The game object calling the event</param>
        public void PlayAudio(GameObject caller)
        {
            audioEvent.PlaySound(caller);
        }
    }
}