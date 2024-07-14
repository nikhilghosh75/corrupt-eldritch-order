using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FeatNotification : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        notificationImage = GetComponent<Image>();
        EventBus.Subscribe<FeatUnlockedEvent>(OnFeatUnlocked);
    }

    void OnFeatUnlocked(FeatUnlockedEvent e)
    {
        notificationText.text = $"Feat Achieved: {e.feat.Name}\n{e.feat.RewardString()} unlocked";
        StartCoroutine(PlayAnimation());
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
