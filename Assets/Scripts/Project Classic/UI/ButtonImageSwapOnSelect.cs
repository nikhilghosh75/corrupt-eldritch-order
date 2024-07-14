using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonImageSwapOnSelect : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite defaultImage, selectedImage;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    void OnEnable()
    {
        //button.image.sprite = defaultImage;
    }

    public void OnSelect(BaseEventData eventData)
    {
        button.image.sprite = selectedImage;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        button.image.sprite = defaultImage;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Nothing for now
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Nothing for now
    }
}
