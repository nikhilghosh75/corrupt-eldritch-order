using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FadingText : MonoBehaviour
{
    private TextMeshProUGUI textComponent;

    public float duration = 1f;
    public float minOpacity = 0.5f;
    public float maxOpacity = 1f;

    private void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();

        if (textComponent != null)
            StartCoroutine(OscillateTextOpacity());
    }

    private IEnumerator OscillateTextOpacity()
    {
        while (true)
        {
            float t = Mathf.Sin(Time.time / duration * Mathf.PI * 2) * 0.5f + 0.5f;
            float opacity = Mathf.Lerp(minOpacity, maxOpacity, t);
            SetTextOpacity(opacity);

            yield return null;
        }
    }

    private void SetTextOpacity(float opacity)
    {
        if (textComponent != null)
        {
            Color color = textComponent.color;
            color.a = opacity; // Set the alpha value
            textComponent.color = color; // Apply the new color with the modified alpha
        }
    }
}
