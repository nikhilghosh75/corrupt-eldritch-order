using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInvincibilityController : MonoBehaviour
{
    PlayerHealth playerHealth;
    Coroutine invincibilityCR;

    bool isPermanent = false;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void ApplyInvincibility(float duration)
    {
        if (isPermanent) return;

        if (invincibilityCR != null) { StopCoroutine(invincibilityCR); }
        invincibilityCR = StartCoroutine(ApplyInvincibilityCR(duration));
    }

    public void ApplyInvincibilityPermanent(bool on)
    {
        if (invincibilityCR != null) { StopCoroutine(invincibilityCR); }
        isPermanent = on;
        playerHealth.isInvincible = on;
    }

    IEnumerator ApplyInvincibilityCR(float duration)
    {
        playerHealth.isInvincible = true;
        yield return new WaitForSeconds(duration);
        playerHealth.isInvincible = false;
    }
}
