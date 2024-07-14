/*
 * Allows you to control the volume through UnityEngine.UI.Slider objects and AudioParams
 * @ Jacob Shreve '?, @ Nikhil Ghosh '24
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WSoft.Audio
{
    [System.Serializable]
    public struct VolumeData
    {
        [Tooltip("The slider that controls the volume")]
        public Slider volumeSlider;

        public string settingName;

        [Tooltip("Information about the audio parameter to change")]
#if WSOFT_WWISE
        public WWise.AudioParam param;
#elif WSOFT_FMOD
        public FMOD.AudioParam param;
#else
        public Default.AudioParam param;
#endif

        /// <summary>
        /// Set the RTPC and PlayerPrefs value for the volume when the value on a slider has changed
        /// </summary>
        /// <param name="newValue">The new value for the RTPC</param>
        public void SetVolumeFromValue(float newValue)
        {
            Debug.Log(settingName + " set to " + newValue);
            volumeSlider.SetValueWithoutNotify(newValue);
            PlayerPrefs.SetFloat(settingName, newValue);

            param.Set(newValue);
        }
    }

    public class VolumeController : MonoBehaviour
    {
        [Tooltip("A list of volumes that can be controlled")]
        public List<VolumeData> volumesToControl;

        [SerializeField] float defaultVolume = 0.7f;
        [SerializeField] bool playerPrefsEnabled = true;

        void Awake()
        {
            foreach(VolumeData volume in volumesToControl)
            {
                if (playerPrefsEnabled && PlayerPrefs.HasKey(volume.settingName))
                {
                    volume.SetVolumeFromValue(PlayerPrefs.GetFloat(volume.settingName));
                }
                else
                {
                    volume.SetVolumeFromValue(defaultVolume);
                }
                volume.volumeSlider.onValueChanged.AddListener(volume.SetVolumeFromValue);
            }
        }
    }
}