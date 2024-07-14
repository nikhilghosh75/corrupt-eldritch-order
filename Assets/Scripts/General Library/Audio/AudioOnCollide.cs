/*
 * An extension of the AudioOnCollide2D script, but used collisions in 3D
 * @ Nikhil Ghosh '24
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Audio
{
    public class AudioOnCollide : MonoBehaviour
    {
        [Tooltip("The datas used for Audio Collisions")]
        public CollisionAudio[] audioDatas;

        /// <summary>
        /// Opon each collision, check to see if the Audio should play.
        /// </summary>
        void OnCollisionEnter(Collision collision)
        {
            foreach (CollisionAudio audioData in audioDatas)
            {
                bool didAudioPlay = PlayAudioData(audioData, collision.gameObject);
                if (didAudioPlay)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Check if the audio should play, based on the collided object and the audio data, and play the audio if it should.
        /// </summary>
        /// <param name="audioData">The data of the audio</param>
        /// <param name="collidedObject">The object collided with</param>
        /// <returns>Did the audio actually play</returns>
        bool PlayAudioData(CollisionAudio audioData, GameObject collidedObject)
        {
            switch (audioData.comparisonMode)
            {
                case CollisionComparisonMode.NONE:
                    // Since there is no comparison, play the audio automatically
                    audioData.audioEvent.PlayAudio(this.gameObject);
                    return true;
                case CollisionComparisonMode.LAYER:
                    // If the game object has the layer in the Layermask, play the sound. Otherwise, don't play the sound.
                    if (HasLayer(collidedObject, audioData.layerMask))
                    {
                        audioData.audioEvent.PlayAudio(this.gameObject);
                        return true;
                    }
                    return false;
                case CollisionComparisonMode.TAG:
                    // If the game object has the specified tag, play the sound. Otherwise, don't play the sound.
                    if (HasTag(collidedObject, audioData.tag))
                    {
                        audioData.audioEvent.PlayAudio(this.gameObject);
                        return true;
                    }
                    return false;
                case CollisionComparisonMode.BOTH:
                    // If the game object has the layer in the Layermask and the specified tag, play the sound. Otherwise, don't play the sound.
                    if (HasTag(collidedObject.gameObject, audioData.tag) && HasLayer(collidedObject, audioData.layerMask))
                    {
                        audioData.audioEvent.PlayAudio(this.gameObject);
                        return true;
                    }
                    return false;
            }
            return false;
        }

        bool HasLayer(GameObject testObject, LayerMask mask)
        {
            return WSoft.Math.LayermaskFunctions.IsInLayerMask(mask, testObject.layer);
        }

        bool HasTag(GameObject testObject, string tag)
        {
            return testObject.CompareTag(tag);
        }
    }
}
