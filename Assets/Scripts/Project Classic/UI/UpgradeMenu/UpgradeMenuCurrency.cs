using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeMenuManager : MonoBehaviour
{
    int currencyDelta = 0;
    private TextMeshProUGUI currencyText;
       

    // Start is called before the first frame update
    void Start()
    {
        currencyText = GetComponent<TextMeshProUGUI>();
        currencyText.text = "Coin: " + RunManager.Instance.permanentCurrency;
        EventBus.Subscribe<CurrencyAddedEvent>(OnCurrencyAdded);
    }

    void OnCurrencyAdded(CurrencyAddedEvent e) 
    {
        currencyText.text = "Coins: " + RunManager.Instance.permanentCurrency;
        currencyDelta += e.addedPermanentCurrency;
    }
}
