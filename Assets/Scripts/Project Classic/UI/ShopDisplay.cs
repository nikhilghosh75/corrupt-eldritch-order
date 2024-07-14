using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopDisplay : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    public static ShopDisplay instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void ShowDisplay(string title, string description)
    {
        titleText.text = title;
        descriptionText.text = description;
    }

    public void HideDisplay()
    {
        gameObject.SetActive(false);
    }
}
