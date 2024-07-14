using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradePurchasedEvent
{

}

public class UpgradeButton : MonoBehaviour
{
    public int level;

    Button upgradeButton;
    Image buttonImage;

    [SerializeField] Color lockedColor = Color.red;
    [SerializeField] Color purchasedColor = Color.green;

    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI costText;

    UpgradeTree tree;

    private void Awake()
    {
        upgradeButton = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
    }

    public void InitializeButton(UpgradeTree _tree) 
    {
        tree = _tree;
        int permCurrency = RunManager.Instance.permanentCurrency;

        if (tree.GetUpgradeCost(level) > permCurrency && !tree.GetIsBought(level))
        {
            upgradeButton.interactable = false;
            buttonImage.color = lockedColor;
        }
        else if (tree.GetIsBought(level))
        {
            upgradeButton.interactable = false;
            buttonImage.color = purchasedColor;
        }
        // Don't let the user purchase an upgrade if the previous one hasn't been purcased
        else if (level > 1 && !tree.GetIsBought(level - 1))
        {
            upgradeButton.interactable = false;
            buttonImage.color = lockedColor;
        }
        else 
        {
            upgradeButton.interactable = true;
            buttonImage.color = Color.white;
        }
        costText.text = tree.GetUpgradeCost(level).ToString();
        levelText.text = "LVL " + level.ToString();
    }

    public void PurchaseUpgrade() 
    {
        int cost = tree.GetUpgradeCost(level);
        if (cost <= RunManager.Instance.permanentCurrency) 
        {
            tree.PurchaseNextUpgrade();
            RunManager.Instance.AddCurrency(-cost, 0);
            EventBus.Publish(new UpgradePurchasedEvent());
            upgradeButton.interactable = false;
            buttonImage.color = purchasedColor;
        }
    }
}
