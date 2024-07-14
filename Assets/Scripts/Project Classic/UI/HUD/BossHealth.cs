using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    private BossHealthSystem bossHealth;
    private float bossMaxHealth;

    public CanvasGroup bossHealthUI;
    public float fadeDuration = 1f;

    private Image healthBar;
    public float fillDuration = 1f;

    bool isFighting = false;


    // Start is called before the first frame update
    void Start()
    {
        bossHealthUI.alpha = 0f;

        healthBar = GetComponent<Image>();
        healthBar.type = Image.Type.Filled;
        healthBar.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFighting)
        {
            healthBar.fillAmount = bossHealth.Current / bossMaxHealth;
            if (bossHealth.Current <= 0)
                isFighting = false;
        }
        else 
        {
            StartCoroutine(FadeOut());
        }
    }

    public void ActivateHealthBar() 
    {
        bossHealth = GameObject.FindObjectOfType<BossBehavior>().GetComponent<BossHealthSystem>();
        bossMaxHealth = bossHealth.maxHealth;
        StartCoroutine(FadeIn());
        StartCoroutine(FillHealthBar());
        isFighting = true;
    }

    IEnumerator FadeIn() 
    {
        float alphaChangePerSecond = 1f / fadeDuration;

        while (bossHealthUI.alpha < 1f) 
        {
            bossHealthUI.alpha += alphaChangePerSecond * Time.deltaTime;
            bossHealthUI.alpha = Mathf.Min(bossHealthUI.alpha, 1f); //Don't go over 1f for alpha

            yield return null;
        }
    }

    IEnumerator FillHealthBar() 
    {
        float fillChangePerSecond = 1f / fillDuration;

        if (healthBar == null)
        {
            healthBar = GetComponent<Image>();
            healthBar.type = Image.Type.Filled;
            healthBar.fillAmount = 0;
        }

        while (healthBar.fillAmount < 1f) 
        {
            healthBar.fillAmount += fillChangePerSecond * Time.deltaTime;
            healthBar.fillAmount = Mathf.Min(healthBar.fillAmount, 1f);

            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        float alphaChangePerSecond = 1f / fadeDuration;

        while (bossHealthUI.alpha > 0f)
        {
            bossHealthUI.alpha -= alphaChangePerSecond * Time.deltaTime;
            bossHealthUI.alpha = Mathf.Max(bossHealthUI.alpha, 0f); //Don't go over 1f for alpha

            yield return null;
        }
    }
}
