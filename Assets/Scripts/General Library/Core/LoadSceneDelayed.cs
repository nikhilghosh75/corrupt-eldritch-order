/*
 * Allows UnityEvents to load a scene with a delay
 * @ Max Perraut '20
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WSoft.Core
{
    public class LoadSceneDelayed : MonoBehaviour
    {
        [Tooltip("The delay between when the function call and the actual loading of the scene, in seconds")]
        [SerializeField] private float delay = 0f;

        /// <summary>
        /// Loads a scene by it's name on a delay.
        /// </summary>
        /// <param name="sceneName">The name of the scene to be loaded</param>
        public void LoadByName(string sceneName)
        {
            StartCoroutine(LoadDelayed(sceneName));
        }

        private IEnumerator LoadDelayed(string sceneName)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(sceneName);
        }
    }
}
