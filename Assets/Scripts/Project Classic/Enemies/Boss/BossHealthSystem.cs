using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Combat;

public class BossHealthSystem : HealthSystem
{
    [Tooltip("The maximum amount of health. Also the starting amount")]
    public int maxHealth;

    [Tooltip("The duration of the iFrames, if iFrames are enabled")]
    public float iframesDuration = 1f;

    public int maximumDamage = 100;

    // The time at which the iFrames end, in Unity time
    private float iframesEnd = -1f;

    protected override void Initialize()
    {
        base.Initialize();

        current = maxHealth;
    }

    protected override bool ApplyDamage(int amount, object obj = null)
    {
        // Black damage if the iframes are stil active
        if (Time.time < iframesEnd)
        {
            return false;
        }

        iframesEnd = Time.time + iframesDuration;

        current -= Mathf.Min(amount, maximumDamage);

        if (current <= 0)
        {
            current = 0;
            Die();
        }

        return true;
    }

    protected override bool ApplyHeal(int amount, object obj = null)
    {
        return false;
    }
}
