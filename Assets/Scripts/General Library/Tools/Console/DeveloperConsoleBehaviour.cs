using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*
 * A MonoBehaviour enabling a development console in the game
 * Written by Brandon Schulz '22, William Bostick '20, Nikhil Ghosh '24, Crystal Lee :)
 */

namespace WSoft.Tools.Console
{
    public class DeveloperConsoleBehaviour : MonoBehaviour
    {
        /*
        // This is no longer necessary due to reflection, but it will be kept just in case things don't work
        private List<ConsoleCommand> commands = new List<ConsoleCommand>()
        {
            new ChangeSceneCommand(),
            new HelpCommand(),
            new LogCommand(),
            new SetTimeScale()
        };
        */

        #region Public Variables
        public UnityEvent OnConsoleClose;
        public UnityEvent OnConsoleOpen;
        #endregion

        #region Serialized Variables
        [Header("UI")]
        public GameObject uiCanvas = null;
        public TMP_InputField inputField = null;
        public TMP_Text autocompleteText = null;

        [Header("Input")]
        public Key openKey;
        public Key closeKey;
        public Key autocompleteKey;
        #endregion

        #region Private Variables (and Constructor)
        private float pausedTimeScale;
        private bool pausedCursorVisibility;
        private CursorLockMode pausedLockMode;
        private static DeveloperConsoleBehaviour instance;
        public static DeveloperConsoleBehaviour Instance { get { return instance; } }
        private DeveloperConsole developerConsole;
        public DeveloperConsole DeveloperConsole
        {
            get
            {
                if (developerConsole != null) { return developerConsole; }
                return developerConsole = new DeveloperConsole();
            }
        }
        #endregion

        private void Awake()
        {
            // playerInput = GetComponentInParent<PlayerInputController>();
            // aimAndShooting = GetComponentInParent<PlayerAimingAndShooting>();
            inputField.onEndEdit.AddListener(ProcessCommand);
            inputField.onValueChanged.AddListener(AutoComplete);

            instance = this;

            uiCanvas.SetActive(false);
        }

        private void Update()
        {
            // close
            if (Keyboard.current[closeKey].wasPressedThisFrame && uiCanvas.activeSelf)
            {
                CloseConsole();

            }
            // open
            else if (Keyboard.current[openKey].wasPressedThisFrame && uiCanvas.activeSelf == false)
            {
                OpenConsole();
                AutoComplete(inputField.text);
            }

            if (Keyboard.current[autocompleteKey].wasPressedThisFrame && uiCanvas.activeSelf)
            {
                List<string> autoCompletedMatches = DeveloperConsole.AutoComplete(inputField.text);
                if (autoCompletedMatches.Count != 0)
                {
                    inputField.text = autoCompletedMatches[0] + " ";
                    inputField.caretPosition = inputField.text.Length;
                }
            }
        }

        private void CloseConsole()
        {
            uiCanvas.SetActive(false);

            Time.timeScale = pausedTimeScale;
            Cursor.lockState = pausedLockMode;
            Cursor.visible = pausedCursorVisibility;

            OnConsoleClose.Invoke();
        }

        private void OpenConsole()
        {
            pausedTimeScale = Time.timeScale;
            pausedCursorVisibility = Cursor.visible;
            pausedLockMode = Cursor.lockState;

            Time.timeScale = 0;
            uiCanvas.SetActive(true);
            inputField.ActivateInputField();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            OnConsoleOpen.Invoke();
        }

        public void ProcessCommand(string inputValue)
        {
            DeveloperConsole.ProcessCommand(inputValue);

            inputField.text = string.Empty;
            CloseConsole();
        }

        public void AutoComplete(string value)
        {
            List<string> matchingCommands = DeveloperConsole.AutoComplete(value);
            string autocompleteString = "";
            foreach (string matchingCommand in matchingCommands)
            {
                autocompleteString += matchingCommand + "\n";
            }

            autocompleteText.text = autocompleteString;
        }
    }
}