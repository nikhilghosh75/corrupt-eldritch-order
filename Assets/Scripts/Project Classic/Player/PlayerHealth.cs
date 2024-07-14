using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using WSoft.Combat;

public class PlayerHealth : BasicHealth
{
    public int armor = 0;

    private WSoft.Camera.CameraShake shaker;
    private WSoft.Camera.CameraShakeSettings shakerSettings;
    public float camShakeDuration = 1f, camShakeAmplitude = 1f, camShakeFrequency = 1f;
    public Vector3 camPivotOffset;

    private void Awake()
    {
        ResetHealth();
        SetShakeSettings();
    }

    private void OnDisable()
    {
        OnUpdateHealth(-current);
    }

    public void ResetHealth()
    {
        current = maxHealth;
        OnUpdateHealth(0);
    }

    protected override bool ApplyHeal(int health, object obj = null)
    {
        if (current == maxHealth)
            return false;

        OnUpdateHealth(health);
        return base.ApplyHeal(health, obj);
    }

    protected override bool ApplyDamage(int health, object obj = null)
    {
        int damage = Mathf.Max(health - armor, 0);
        OnUpdateHealth(-damage);
        
        if (SettingsLoader.Settings.screenshakeEnabled)
        {
            shaker.Shake(shakerSettings); // camera will shake upon receiving damage
        }

        return base.ApplyDamage(damage, obj);
    }

    public bool CanBeDamaged() => IsInIFrames() || isInvincible;

    void OnUpdateHealth(int delta)
    {
        // Don't send damage events if player is invincible or has IFrames
        if (delta < 0 && (IsInIFrames() || isInvincible))
            return;

        EventBus.Publish<UpdateHealthEvent>(new UpdateHealthEvent(current, delta));
    }

    public void ChangeMaxHealth(float multiplier)
    {
        maxHealth = (int)(maxHealth * multiplier);
    }

    public void SetMaxHealth(int hp)
    {
        maxHealth = hp;
    }

    public void SetMaxAndHeal(int hp)
    {
        SetMaxHealth(hp);
        ApplyHeal(2);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
    }

    public void DecreaseMaxHealth(int amount)
    {
        maxHealth -= amount;

        if (current > maxHealth)
        {
            current = maxHealth;
        }
    }

    private void SetShakeSettings()
    {
        TryGetComponent<WSoft.Camera.CameraShake>(out shaker);
        shakerSettings.shakeDuration = camShakeDuration;
        shakerSettings.shakeAmplitude = camShakeAmplitude;
        shakerSettings.shakeFrequency = camShakeFrequency;
        shakerSettings.pivotOffset = camPivotOffset;
    }

    public void DisableIn(float time)
    {
        Invoke("Disable", time);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}

public class UpdateHealthEvent
{
    public int health;
    public int healthDelta;

    public UpdateHealthEvent(int health, int healthDelta)
    {
        this.health = health;
        this.healthDelta = healthDelta;
    }
}
