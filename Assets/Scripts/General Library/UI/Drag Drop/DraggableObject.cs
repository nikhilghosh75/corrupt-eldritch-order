using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/*
 * A part of the drag/drop system. Goes on object that can be dragged.
 * When dragged onto a drop target that supports this type, it sticks to the DropTarget
 * Written by Nikhil Ghosh '24
 */

namespace WSoft.UI
{
    public class DraggableObject : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [Tooltip("The type of object that this is")]
        public string type;

        [Tooltip("Should the object reset to the original position even if it is accepted")]
        public bool revertToOriginalPosition;

        public UnityEvent OnDragBegin;
        public UnityEvent OnDragEnd;

        Image image;
        RectTransform rectTransform;
        Canvas canvas;

        Vector2 originalPosition;

        // Start is called before the first frame update
        void Start()
        {
            image = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();

            // This may be a bad idea if there are multiple canvases
            canvas = GameObject.FindObjectOfType<Canvas>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (image != null)
            {
                image.raycastTarget = false;
            }
            originalPosition = rectTransform.anchoredPosition;
            OnDragBegin.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (image != null)
            {
                image.raycastTarget = true;
            }

            if (revertToOriginalPosition)
            {
                rectTransform.anchoredPosition = originalPosition;
            }
            OnDragEnd.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }
}