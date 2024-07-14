using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerupAnimation : MonoBehaviour
{
    public Vector2 animationStartPosition, animationEndPosition;
    public float transitionDuration = 0.15f;
    public float animationDuration = 1.5f;
    public float fadeOutDuration = 0.15f;
    public TextMeshProUGUI notificationText;
    private RectTransform rectTransform;
    private Image notificationImage;

    public AnimationCurve curve;

    private Queue<string> animationQueue = new Queue<string>();
    private bool isAnimating = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        notificationImage = GetComponent<Image>();
        EventBus.Subscribe<WeaponPowerupEvent>(onGainWeaponPowerup);
        EventBus.Subscribe<PowerupEvent>(onGainPowerup);
    }

    void onGainPowerup(PowerupEvent e)
    {
        EnqueueAnimation($"{e.powerup.displayName}");
    }

    void onGainWeaponPowerup(WeaponPowerupEvent e)
    {
        EnqueueAnimation($"Weapon Equipped: {e.weaponPowerup.displayName}");
    }

    private void EnqueueAnimation(string message)
    {
        animationQueue.Enqueue(message);
        if (!isAnimating)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    private IEnumerator ProcessQueue()
    {
        while (animationQueue.Count > 0)
        {
            isAnimating = true;
            string message = animationQueue.Dequeue();
            notificationText.text = message;
            yield return StartCoroutine(PlayAnimation());
        }
        isAnimating = false;
    }

    private IEnumerator PlayAnimation()
    {
        yield return StartCoroutine(LerpPositionCoroutine(animationStartPosition, animationEndPosition, transitionDuration));
        yield return new WaitForSeconds(animationDuration);
        yield return StartCoroutine(FadeNotification(1, 0, fadeOutDuration));
        rectTransform.anchoredPosition = animationStartPosition;
        Color color = new Color(1, 1, 1, 1);
        notificationImage.color = color;
        notificationText.color = color;
    }

    private IEnumerator LerpPositionCoroutine(Vector2 startPosition, Vector2 targetPosition, float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = curve.Evaluate(timeElapsed / duration);
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
    }

    private IEnumerator FadeNotification(float startOpacity, float endOpacity, float duration)
    {
        float timeElapsed = 0f;

        Color color = notificationImage.color;
        color.a = startOpacity;
        notificationImage.color = color;
        notificationText.color = color;

        while (timeElapsed < duration)
        {
            float t = curve.Evaluate(timeElapsed / duration);
            float a = Mathf.Lerp(startOpacity, endOpacity, t);
            color.a = a;
            notificationImage.color = color;
            notificationText.color = color;
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        color.a = endOpacity;
        notificationImage.color = color;
        notificationText.color = color;
    }
}
