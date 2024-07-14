using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/*
 * A utility class that allows UnityEvents to be invoked when the mouse cursor is
 * hovering over a RectTransform.
 * Written by Nikhil Ghosh '24
 */

namespace WSoft.UI
{
    public class HoverEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public UnityEvent OnHoverBegin;
        public UnityEvent OnHoverStay;
        public UnityEvent OnHoverEnd;

        bool isHovering = false;

        void Update()
        {
            if (isHovering)
            {
                OnHoverStay.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHoverBegin.Invoke();
            isHovering = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnHoverEnd.Invoke();
            isHovering = false;
        }
    }
}