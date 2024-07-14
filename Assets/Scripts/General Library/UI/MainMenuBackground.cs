/* Allows to change an image to a different Sprite based on a parameter
* Written by William Bostick '20, Nikhil Ghosh '24
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WSoft.UI
{
    public class MainMenuBackground : MonoBehaviour
    {
        [Tooltip("The index of the sprite to start with by default")]
        [SerializeField] int artIndex;

        /// <summary>
        /// holds the Sprites corresponding to different checkpoints  
        /// set this in the inspector
        /// </summary>
        [Tooltip("The sprites corresponding to the different main menu backgrounds")]
        [SerializeField] List<Sprite> backgroundImageArray;

        /// <summary>
        /// contains a reference to the image that will be changed
        ///   by this script  
        /// set this in the inspector
        /// </summary>
        [Tooltip("A reference to the image that will be changed")]
        [SerializeField] Image backgroundImg;

        [Tooltip("The name of the PlayerPrefs key for the background to display. Set to empty for ")]
        [SerializeField] string playerPrefName;

        /// <summary>
        /// on awake, set the background image to the art for the checkpoint the 
        ///   player is at
        /// </summary>
        void Awake()
        {
            int index = GetCheckpontArtIndex();
            UpdateBackgroundImage(index);

        }

        /// <summary>
        /// returns the index in the *backgroundImageArray* that the main menu should be set to
        /// </summary>
        int GetCheckpontArtIndex()
        {
            if (PlayerPrefs.HasKey(playerPrefName))
            {
                return PlayerPrefs.GetInt(playerPrefName);
            }

            return artIndex;
        }

        /// <summary>
        /// changes the displayed sprite to the sprite in the preset array 
        ///   *backgroundImg* at index *index*
        /// </summary>
        public void UpdateBackgroundImage(int index)
        {
            artIndex = index;
            backgroundImg.overrideSprite = backgroundImageArray[artIndex];
        }

        /// <summary>
        /// Changes the image to whatever Sprite *s* is provided
        /// </summary>
        public void UpdateBackgroundImage(Sprite s)
        {
            artIndex = -1;
            backgroundImg.overrideSprite = s;
        }
    }
}