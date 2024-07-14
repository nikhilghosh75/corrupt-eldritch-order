using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * A trigger that enables the tooltip whenever the mouse hovers above
 * Written by Nikhil Ghosh '24
 */


namespace WSoft.UI
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string title;
        public string content;
        public Color backgroundColor = Color.black;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Tooltip.SetActive(true);
            Tooltip.SetTootip(title, content, backgroundColor);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.SetActive(false);
        }
    }
}