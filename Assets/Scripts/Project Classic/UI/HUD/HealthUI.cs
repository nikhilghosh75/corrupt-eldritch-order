using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private PlayerHealth healthScript;
    [SerializeField]
    private Sprite fullHeart;
    [SerializeField]
    private Sprite emptyHeart;

    [SerializeField]
    private Color damageParticleColor;
    [SerializeField]
    private Color healParticleColor;

    [SerializeField]
    private ParticleSystem ps;

    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private GameObject healthTextGameObject;


    void Start()
    {
        EventBus.Subscribe<UpdateHealthEvent>(OnUpdateHealthEvent);

        SetHealthText(healthScript.maxHealth);
    }

    private void OnUpdateHealthEvent(UpdateHealthEvent e)
    {
        int newHealth = e.health + e.healthDelta;

        if (e.healthDelta < 0) 
        {
            ps.startColor = damageParticleColor;
        }

        else if (e.healthDelta > 0)
        {
            ps.startColor = healParticleColor;
        }

        if (e.healthDelta != 0)
            ps.Play();

        SetHealthText(newHealth);
    }

    private void SetHealthText(int health)
    {
        healthText.text = $"+{health.ToString()}";
    }
}
