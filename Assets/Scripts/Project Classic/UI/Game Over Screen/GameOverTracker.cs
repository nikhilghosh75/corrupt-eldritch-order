using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameOverTracker : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject pauseMenu;
    public TextMeshProUGUI gameOverText;

    [Header("Shopkeeper Death Dialogue")]
    public string[] OnDeathDialogueOptions;

    // Start is called before the first frame update
    void Start()
    {
        gameOverScreen.SetActive(false);
        EventBus.Subscribe<UpdateHealthEvent>(OnHealthChanged);

        gameOverText.text = OnDeathDialogueOptions[Random.Range(0, OnDeathDialogueOptions.Length)] + " - Kraig";
    }

    void OnHealthChanged(UpdateHealthEvent e)
    {
          if (e.health + e.healthDelta <= 0)
          {
            gameOverScreen.SetActive(true);
            pauseMenu.SetActive(false);
            Time.timeScale = 0;
            PlayerController.Instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
          }
    }
}
