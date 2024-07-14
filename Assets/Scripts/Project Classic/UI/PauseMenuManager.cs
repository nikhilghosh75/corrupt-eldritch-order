using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseEvent
{

}

public class ResumeEvent
{

}
public class RestartLevelEvent
{

}

public class PauseMenuManager : MonoBehaviour
{
    public TMP_Text seedText;
    public GameObject pauseMenu;
    public GameObject background;
    public List<GameObject> menusToDisableOnStart = new List<GameObject>();
    public List<GameObject> menusToEnableOnStart = new List<GameObject>();
    public InputActionReference pauseAction;
    public InputActionReference restartLevel;
    public GameObject levelMusic;

    public Button resumeButton;
    public GamepadButton gamepadButton;
    public string key;

    bool gamePaused = false;
    bool unpauseCooldown = false;

    void Start()
    {
        for (int i = 0; i < menusToDisableOnStart.Count; i++)
        {
            menusToDisableOnStart[i].SetActive(false);
        }

        for (int i = 0; i < menusToEnableOnStart.Count; i++)
        {
            menusToEnableOnStart[i].SetActive(true);
        }

        EventBus.Subscribe<RestartLevelEvent>(RestartLevel);
        EventBus.Subscribe<PauseEvent>(PauseGame);
    }

    void Update()
    {
        if (pauseMenu.activeInHierarchy && !unpauseCooldown) 
        {
            if (!string.IsNullOrEmpty(key) && Keyboard.current.FindKeyOnCurrentKeyboardLayout(key).wasPressedThisFrame)
            {
                resumeButton.onClick.Invoke();
            }
            else if (Gamepad.current[gamepadButton].wasPressedThisFrame)
            {
                resumeButton.onClick.Invoke();
            }
        }
    }

    void PauseGame(PauseEvent e)
    {
        if (!gamePaused && !unpauseCooldown)
        {
            gamePaused = true;
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
            background.SetActive(true);
            StartCoroutine(ResetUnpauseCooldown());
            seedText.text = "Seed: " + GameManager.Instance.seed;
            PlayerInput.GetPlayerByIndex(0).SwitchCurrentActionMap("UI");
        }
    }

    public void ResumeGame()
    {
        if (gamePaused)
        {
            gamePaused = false;
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            background.SetActive(false);
            StartCoroutine(ResetUnpauseCooldown());
            PlayerInput.GetPlayerByIndex(0).SwitchCurrentActionMap("Player");
        }
    }

    IEnumerator ResetUnpauseCooldown() 
    {
        unpauseCooldown = true;
        yield return new WaitForSecondsRealtime(0.1f);
        unpauseCooldown = false;
    }

    void RestartLevel(RestartLevelEvent e)
    {
        if(Time.timeScale != 0f){
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);

            PlayerInput.GetPlayerByIndex(0).SwitchCurrentActionMap("Player");
            SceneManager.LoadScene("Default");
        }
    }

    public void ExitToMainMenu()
    {
        Destroy(levelMusic);
        Time.timeScale = 1f;
        Debug.Log(Time.timeScale);
        SceneManager.LoadScene("Main Menu");
    }
}
