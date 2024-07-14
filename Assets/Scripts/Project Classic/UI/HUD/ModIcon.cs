using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModActivatedEvent
{
    public WeaponMod modActivated;

    public ModActivatedEvent(WeaponMod _modActivated)
    {
        modActivated = _modActivated;
    }
}

public class ModIcon : MonoBehaviour
{
    public WeaponMod currentMod;

    public float timeToMax;
    public float timeAtMax;
    public float timeToMin;
    public float timeAtMin;
    public float timeToOne;
    public float maxScale;
    public float minScale;

    bool isAnimating = false;

    void Start()
    {
        EventBus.Subscribe<ModActivatedEvent>(OnModActivated);
    }

    void OnModActivated(ModActivatedEvent e)
    {
        if (currentMod == null)
        {
            return;
        }

        if (e.modActivated == currentMod || e.modActivated.displayName == currentMod.displayName)
        {
            StartCoroutine(PerformAnimation());
        }
    }

    IEnumerator PerformAnimation()
    {
        if (isAnimating)
            yield break;

        isAnimating = true;

        for (float t = 0; t < timeToMax; t += Time.deltaTime)
        {
            transform.localScale = Mathf.Lerp(1f, maxScale, t / timeToMax) * Vector3.one;
            yield return null;
        }

        yield return timeAtMax;

        for (float t = 0; t < timeToMin; t += Time.deltaTime)
        {
            transform.localScale = Mathf.Lerp(maxScale, minScale, t / timeToMin) * Vector3.one;
            yield return null;
        }

        yield return timeAtMin;

        for (float t = 0; t < timeToMax; t += Time.deltaTime)
        {
            transform.localScale = Mathf.Lerp(minScale, 1f, t / timeToMax) * Vector3.one;
            yield return null;
        }

        isAnimating = false;
    }
}
