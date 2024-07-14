using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    public TextMeshProUGUI currencyText;
    void Update()
    {
        if (RunManager.Instance != null)
            currencyText.text = RunManager.Instance.runCurrency.ToString();
    }
}
