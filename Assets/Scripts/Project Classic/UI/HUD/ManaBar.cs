using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WSoft.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField]
    private PlayerMana playerMana;

    [SerializeField]
    private Image middleImage;

    [SerializeField]
    private Image topImage;

    [SerializeField]
    private float visualLerpSpeed = 1f;

    private float displayedMana;

    private void Start()
    {
        displayedMana = playerMana.GetCurrentMana();
    }

    private void Update()
    {
        UpdateManaUI();
    }

    private void UpdateManaUI()
    {
        float currentMana = playerMana.GetCurrentMana();
        float maxMana = playerMana.maxMana;

        displayedMana = Mathf.Lerp(displayedMana, currentMana, Time.deltaTime * visualLerpSpeed);

        float manaPercentage = displayedMana / maxMana;
        float middleImageHeight = Mathf.Lerp(0, 75f, manaPercentage);
        float topImageYPosition = middleImageHeight;

        middleImage.rectTransform.SetHeight(middleImageHeight);
        topImage.rectTransform.anchoredPosition = new Vector2(0, topImageYPosition);
    }
}