/*
 * Shakes the camera based on the CameraShakeSettings
 * @ Matthew Rader '19
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace WSoft.Camera
{
    // A struct containing the shake settings
    public struct CameraShakeSettings
    {
        [Tooltip("The duration of the shaking")]
        public float shakeDuration;

        [Tooltip("The amplitude of the shaking. 1 is normal. Larger magnitudes increase the strength of the shaking")]
        public float shakeAmplitude;

        [Tooltip("The frequency of the shaking. 1 is normal. Larger magnitudes increase the number of shakes")]
        public float shakeFrequency;

        [Tooltip("The duration of the shaking, in seconds")]
        public Vector3 pivotOffset;
    }

    public class CameraShake : MonoBehaviour
    {
        [Tooltip("The Cinemachine Virtual Camera")]
        [SerializeField]
        private CinemachineVirtualCamera vcam;

        private CinemachineBasicMultiChannelPerlin noise;

        void Awake()
        {
            noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        /// <summary>
        /// Shake the camera upon a public function
        /// </summary>
        /// <param name="shakeSettings">The seetings to shake the camera on</param>
        public void Shake(CameraShakeSettings shakeSettings)
        {
            if (!vcam || !noise)
            {
                CinemachineBrain brain = UnityEngine.Camera.main.GetComponent<CinemachineBrain>();
                vcam = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
                if (vcam)
                    noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }

            if (vcam)
                StartCoroutine(DoCameraShake(shakeSettings));
        }

        /// <summary>
        /// The coroutine responsible for the actual shaking of the camera
        /// </summary>
        /// <param name="shakeSettings">The seetings to shake the camera on</param>
        IEnumerator DoCameraShake(CameraShakeSettings shakeSettings)
        {
            noise.m_AmplitudeGain = shakeSettings.shakeAmplitude;
            noise.m_FrequencyGain = shakeSettings.shakeFrequency;
            noise.m_PivotOffset = shakeSettings.pivotOffset;

            yield return new WaitForSeconds(shakeSettings.shakeDuration);

            noise.m_AmplitudeGain = 0;
            noise.m_FrequencyGain = 0;
            noise.m_PivotOffset = Vector3.zero;
        }
    }
}
