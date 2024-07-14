using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*
 * A tooltip that appears whenever the mouse hovers above a tooltip trigger
 * Written by Nikhil Ghosh '24
 */

namespace WSoft.UI
{
    public class Tooltip : MonoBehaviour
    {
        [HideInInspector]
        public Image background;

        public Text title;
        public Text content;

        public int characterWrapLimit;

        public static Tooltip instance;

        LayoutElement layoutElement;

        void Awake()
        {
            instance = this;

            background = GetComponent<Image>();
            layoutElement = GetComponent<LayoutElement>();

            gameObject.SetActive(false);
        }

        void Update()
        {
            Vector2 position = Mouse.current.position.ReadValue();
            transform.position = position;
        }

        public static void SetActive(bool active)
        {
            instance.gameObject.SetActive(active);
        }

        public static void SetTootip(string newTitle, string newContent, Color newColor)
        {
            instance.title.text = newTitle;
            if (newTitle.Length == 0)
            {
                instance.title.gameObject.SetActive(false);
            }
            else
            {
                instance.title.gameObject.SetActive(true);
            }

            instance.content.text = newContent;
            if (newContent.Length == 0)
            {
                instance.content.gameObject.SetActive(false);
            }
            else
            {
                instance.content.gameObject.SetActive(true);
            }
            instance.background.color = newColor;

            instance.layoutElement.enabled = (newTitle.Length > instance.characterWrapLimit
                || newContent.Length > instance.characterWrapLimit);
        }
    }
}