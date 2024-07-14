/* Shows a piece of control prompt.
 * @Zhenyuan Zhang
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WSoft.Input.ControlPrompt
{
    public class ControlPromptView : MonoBehaviour
    {
        public ControlPrompt controlPrompt;
        public TMP_Text text;
        public Image image;

        void Awake()
        {
            image.preserveAspect = true;

            text.text = controlPrompt.displayName;
            image.sprite = controlPrompt.Sprite;
        }
    }
}