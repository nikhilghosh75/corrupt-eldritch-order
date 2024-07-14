using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WSoft.FirstPerson
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Settings", order = 51)]
    public class Settings : ScriptableObject
    {
        public static bool GameLoaded { get; private set; } = false;

        public class OnBoolToggle : UnityEvent<bool> { }
        public class OnFloatToggle : UnityEvent<bool> { }

        public OnBoolToggle onCameraToggle;
        public OnFloatToggle onAimAssistStrengthChange;
        public OnFloatToggle onMouseSensitivityChange;
        public OnFloatToggle onFOVChange;

        [SerializeField] bool defaultUseCameraTilt;
        [SerializeField] float defaultAimAssistStrength;
        [SerializeField] float defaultFOV;
        [SerializeField] float defaultMouseSensitivity;

        static bool useCameraTilt_;
        static float aimAssistStrength_;
        static float FOV_;
        static float MouseSensitivity_;

        public bool UseCameraTilt { get { return useCameraTilt_; } set { useCameraTilt_ = value; } }
        public float AimAssistStrength { get { return aimAssistStrength_; } set { aimAssistStrength_ = value; } }
        public float FOV { get { return FOV_; } set { FOV_ = value; } }
        public float MouseSensitivity { get { return MouseSensitivity_; } set { MouseSensitivity_ = value; } }

        void OnEnable()
        {
            SetDefaults();
        }

        public void SetDefaults()
        {
            useCameraTilt_ = defaultUseCameraTilt;
            aimAssistStrength_ = defaultAimAssistStrength;
            FOV_ = defaultFOV;
            MouseSensitivity_ = defaultMouseSensitivity;
            GameLoaded = true;
        }

        public static void SetGameLoadedTrue()
        {
            GameLoaded = true;
        }
    }
}