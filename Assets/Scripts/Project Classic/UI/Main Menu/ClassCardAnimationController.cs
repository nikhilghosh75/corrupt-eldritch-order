using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassCardAnimationController : MonoBehaviour
{
    public int cardIndex = 0;
    public float animationDuration = 0.5f;
    public AnimationCurve animationCurve;
    public string className;
    public Image lockIcon;
    public FeatDisplay featDisplay;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (RunManager.Instance.unlockedClasses.Contains(className))
        {
            lockIcon.enabled = false;
            featDisplay.gameObject.SetActive(false);
        }
    }

    public void MoveAndAnimate(bool reversed = false)
    {
        if (!reversed)
        {
            if (cardIndex == 0)
            {
                Vector2 startPosition = new Vector3(0, 25, 0);
                Vector2 endPosition = new Vector3(0, 1000, 0);
                StartCoroutine(AnimatePosition(startPosition, endPosition));
            }
            else if (cardIndex == 1)
            {
                Vector2 startPosition = new Vector3(-500, 25, 0);
                Vector2 endPosition = new Vector3(0, 25, 0);
                StartCoroutine(AnimatePosition(startPosition, endPosition));
            }
            else if (cardIndex == 2)
            {
                Vector2 startPosition = new Vector3(-1000, 25, 0);
                Vector2 endPosition = new Vector3(-500, 25, 0);
                StartCoroutine(AnimatePosition(startPosition, endPosition));
            }
            else if (cardIndex == 3)
            {
                Vector2 startPosition = new Vector3(-1300, 25, 0);
                Vector2 endPosition = new Vector3(-1000, 25, 0);
                StartCoroutine(AnimatePosition(startPosition, endPosition));
            }
        }
        else
        {
            if (cardIndex == 0)
            {
                Vector2 endPosition = new Vector3(-500, 25, 0);
                Vector2 startPosition = new Vector3(0, 25, 0);
                StartCoroutine(AnimatePosition(startPosition, endPosition));
            }
            else if (cardIndex == 1)
            {
                Vector2 endPosition = new Vector3(-1000, 25, 0);
                Vector2 startPosition = new Vector3(-500, 25, 0);
                StartCoroutine(AnimatePosition(startPosition, endPosition));
            }
            else if (cardIndex == 2)
            {
                Vector2 endPosition = new Vector3(-1300, 25, 0);
                Vector2 startPosition = new Vector3(-1000, 25, 0);
                StartCoroutine(AnimatePosition(startPosition, endPosition));
            }
            else if (cardIndex == 3)
            {
                Vector2 endPosition = new Vector3(0, 25, 0);
                Vector2 startPosition = new Vector3(0, 1000, 0);
                StartCoroutine(AnimatePosition(startPosition, endPosition));
            }
        }
    }
    private IEnumerator AnimatePosition(Vector3 startPosition, Vector3 endPosition)
    {
        float elapsed = 0.0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = animationCurve.Evaluate(elapsed / animationDuration);
            Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, t);
            rectTransform.anchoredPosition = newPosition;
            yield return null;
        }
        rectTransform.anchoredPosition = endPosition;
    }
}
