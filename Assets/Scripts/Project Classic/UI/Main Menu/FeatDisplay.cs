using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FeatDisplay : MonoBehaviour
{
    public TMP_Text nameText;
    public Image progressImage;
    public BaseFeat feat;

    // Start is called before the first frame update
    void Start()
    {
        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDisplay()
    {
        nameText.text = feat.Description;
        progressImage.fillAmount = feat.GetProgress();
    }
}
