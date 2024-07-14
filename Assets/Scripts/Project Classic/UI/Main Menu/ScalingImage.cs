using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScalingImage : MonoBehaviour
{
    private Image imageComponent;

    public float duration = 1f;
    public float minScale = 0.5f;
    public float maxScale = 1.5f;

    private void Start()
    {
        imageComponent = GetComponent<Image>();

        if (imageComponent != null)
            StartCoroutine(OscillateImageScale());
    }

    private IEnumerator OscillateImageScale()
    {
        while (true)
        {
            // Calculate the scale factor over time using a sine wave
            float t = Mathf.Sin(Time.time / duration * Mathf.PI * 2) * 0.5f + 0.5f;
            float scale = Mathf.Lerp(minScale, maxScale, t);
            SetImageScale(scale);

            yield return null;
        }
    }

    private void SetImageScale(float scale)
    {
        if (imageComponent != null)
        {
            // Set the local scale of the image
            imageComponent.rectTransform.localScale = new Vector3(scale, scale, 1f);
        }
    }
}
