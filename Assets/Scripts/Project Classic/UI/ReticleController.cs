using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ReticleController : MonoBehaviour
{
    private RectTransform rectTransform;
    public float inactiveDuration = 2.0f; // Duration in seconds after which the reticle is hidden
    private float lastMoveTime;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        lastMoveTime = Time.time;
        image.enabled = false;
    }

    void Update()
    {
        Vector2 currentMousePosition = Mouse.current.position.ReadValue();

        // Check if the mouse has moved
        if (currentMousePosition != new Vector2(rectTransform.position.x, rectTransform.position.y))
        {
            lastMoveTime = Time.time;
            image.enabled = true;
            rectTransform.position = currentMousePosition;
        }

        // Check if the duration of inactivity has been exceeded
        if (Time.time - lastMoveTime > inactiveDuration)
        {
            image.enabled = false;
        }
    }
}
