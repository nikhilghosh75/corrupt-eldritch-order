/* 
 * This script allows UnityEvents to statically call the Game Manager's load and set scene methods.
 * @ Max Perraut '20
 * 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WSoft.Core
{
    public class LoadScene : MonoBehaviour
    {
        /// <summary>
        /// Loads a scene by name
        /// </summary>
        /// <param name="sceneName">The scene to be loaded</param>
        public void LoadByName(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
