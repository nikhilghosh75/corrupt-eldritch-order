using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyAddedDisplay : MonoBehaviour
{
    RectTransform rectTransform;

    public TMP_Text permanentCurrency;
    public TMP_Text permnanetAddedCurrency;
    public TMP_Text runCurrency;
    public TMP_Text runAddedCurrency;

    public float currencyTickingTime;
    public float stayTime;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        EventBus.Subscribe<CurrencyAddedEvent>(OnCurrencyAdded);

        permanentCurrency.text = RunManager.Instance.permanentCurrency.ToString();
        runCurrency.text = RunManager.Instance.runCurrency.ToString();
        permnanetAddedCurrency.text = "";
        runAddedCurrency.text = "";
    }

    void OnCurrencyAdded(CurrencyAddedEvent e)
    {
        int originalPermanent = RunManager.Instance.permanentCurrency - e.addedPermanentCurrency;
        int originalRun = RunManager.Instance.runCurrency - e.addedRunCurrency;
        StartCoroutine(ShowCurrencyAdded(originalPermanent, originalRun, e.addedPermanentCurrency, e.addedRunCurrency));
    }

    IEnumerator ShowCurrencyAdded(int originalPermanent, int originalRun, int addedPermanent, int addedRun)
    {
        permanentCurrency.text = originalPermanent.ToString();
        runCurrency.text = originalRun.ToString();
        permnanetAddedCurrency.text = "(+" + addedPermanent + ")";
        runAddedCurrency.text = "(+" + addedRun + ")";

        for (float t = 0; t < currencyTickingTime; t += Time.deltaTime)
        {
            int deltaRun = (int)Mathf.Lerp(0, addedRun, t / currencyTickingTime);
            int deltaPermanent = (int)Mathf.Lerp(0, addedPermanent, t / currencyTickingTime);

            permanentCurrency.text = (originalPermanent + deltaPermanent).ToString();
            runCurrency.text = (originalRun + deltaRun).ToString();
            permnanetAddedCurrency.text = "(+" + (addedPermanent - deltaPermanent) + ")";
            runAddedCurrency.text = "(+" + (addedRun - deltaRun) + ")";

            yield return null;
        }

        permanentCurrency.text = (originalPermanent + addedPermanent).ToString();
        runCurrency.text = (originalRun + addedRun).ToString();
        permnanetAddedCurrency.text = "(+0)";
        runAddedCurrency.text = "(+0)";

        yield return new WaitForSeconds(stayTime);

        permnanetAddedCurrency.text = "";
        runAddedCurrency.text = "";
    }
}
