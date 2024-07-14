using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    public void CloseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
