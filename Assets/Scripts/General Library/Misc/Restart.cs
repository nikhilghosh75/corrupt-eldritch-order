using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * This should be fairly self explanatory
 * Useful for Unity Events
 * Written by Natasha Badami '20
 */

namespace WSoft
{
    public class Restart : MonoBehaviour
    {
        public GameObject levelMusic;

        public void RestartScene()
        {
            Destroy(levelMusic);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
