using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public CanvasGroup startCanvas;
    public EventSystem eventSystem;

    public float menuTransitionDuration = 0.25f;

    private CanvasGroup currentCanvas;
    private GameObject lastSelectedItem;

    void Start()
    {
        // Don't don't allow menu to load with time frozen
        if (Time.timeScale != 1)
            Time.timeScale = 1;

        EventBus.Subscribe<MainMenuTransitionEvent>(OnMainMenuTransition);
        currentCanvas = startCanvas;

        // Disable the all except the first two canvases and the event system
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i > 2)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        ActivateCanvas(startCanvas, true);
    }

    private void Update()
    {
        if (EventSystem.current?.currentSelectedGameObject == null)
        {
            // Reselect the last selected object if it exists
            if (lastSelectedItem != null)
            {
                EventSystem.current?.SetSelectedGameObject(lastSelectedItem);
            }
        }
        else
        {
            // Update the last selected object
            lastSelectedItem = EventSystem.current?.currentSelectedGameObject;
        }
    }

    // EventBus is used when non-buttons want to initiate a transition
    private void OnMainMenuTransition(MainMenuTransitionEvent e)
    {
        TransitionToCanvas(e.canvasName);
    }

    public void TransitionToCanvas(string canvasName)
    {
        CanvasGroup nextCanvas = transform.Find(canvasName)?.GetComponent<CanvasGroup>();
        if (nextCanvas != null && nextCanvas != currentCanvas)
        {
            StartCoroutine(Transition(currentCanvas, nextCanvas));
        }
        else
        {
            Debug.LogError("Canvas with name " + canvasName + " not found or is the current canvas.");
        }
    }

    private IEnumerator Transition(CanvasGroup fromCanvas, CanvasGroup toCanvas)
    {
        yield return StartCoroutine(FadeCanvas(fromCanvas, 1, 0, menuTransitionDuration)); // Fade out
        ActivateCanvas(fromCanvas, false);
        yield return new WaitForSeconds(0.3f); // Wait for a bit to make transitions feel better
        ActivateCanvas(toCanvas, true);
        yield return StartCoroutine(FadeCanvas(toCanvas, 0, 1, menuTransitionDuration)); // Fade in
        currentCanvas = toCanvas;
    }

    private IEnumerator FadeCanvas(CanvasGroup canvas, float startAlpha, float endAlpha, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            canvas.alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvas.alpha = endAlpha;
        if (endAlpha == 1)
        {
            Button[] menuButtons = GetComponentsInChildren<Button>(true);
            foreach (Button b in menuButtons)
            {
                b.enabled = true;
            }
        }
    }

    private void ActivateCanvas(CanvasGroup canvas, bool isActive)
    {
        canvas.gameObject.SetActive(isActive);
        if (isActive)
        {
            SetDefaultButton(canvas);
            if (!canvas.name.Contains("Start Screen"))
                // start screen doesn't use the same transition function as normal transitions
                // so don't deactivate in this case
            {
                // deactivate buttons during transition time
                // they will be reactivated in FadeCanvas coroutine
                Button[] menuButtons = GetComponentsInChildren<Button>(true);
                foreach (Button b in menuButtons)
                {
                    b.enabled = false;
                }
            }
        }
    }

    private void SetDefaultButton(CanvasGroup canvas)
    {
        GameObject defaultButton = canvas.GetComponent<DefaultButton>()?.defaultButton;
        if (defaultButton != null)
        {
            eventSystem.SetSelectedGameObject(defaultButton);
        }
        else
        {
            Debug.LogError("No default button setup for " + canvas.name);
        }
    }
}
public class MainMenuTransitionEvent
{
    public string canvasName;

    public MainMenuTransitionEvent(string _canvasName)
    {
        canvasName = _canvasName;
    }
}
