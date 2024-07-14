using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

namespace WSoft.Input
{
    public class InputButton : MonoBehaviour
    {
        Button button;

        // The image representing the gamepad button
        public Image buttonImage;

        // The text representing the keyboard key
        public TMP_Text keyText;

        // A game object to enable whenever the key button is active, in place of text
        public GameObject keyGameObject;

        [Space]
        public ControlPrompt.ControlPrompt controlPrompt;
        public GamepadButton gamepadButton;
        public string key;

        // Start is called before the first frame update
        void Start()
        {
            button = GetComponent<Button>();

            if (keyGameObject == null && keyText != null)
                keyGameObject = keyText.gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            Sprite sprite;
            if (controlPrompt != null)
                sprite = controlPrompt.Sprite;
            else
                sprite = buttonImage.sprite;

            if (sprite == null || Gamepad.current == null)
            {
                keyGameObject.SetActive(true);
                buttonImage.gameObject.SetActive(false);

                if (keyText != null)
                    keyText.text = key;

                if (!string.IsNullOrEmpty(key) && Keyboard.current.FindKeyOnCurrentKeyboardLayout(key).wasPressedThisFrame)
                {
                    button.onClick.Invoke();
                }
            }
            else
            {
                keyGameObject.SetActive(false);
                buttonImage.gameObject.SetActive(true);
                buttonImage.sprite = sprite;
                if (Gamepad.current[gamepadButton].wasPressedThisFrame)
                {
                    button.onClick.Invoke();
                }
            }
        }
    }

}