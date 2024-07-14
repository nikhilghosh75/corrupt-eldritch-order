using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPortal : MonoBehaviour
{
    bool transitioning = false;
    public float fadeLength = 2.0f; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out PlayerController controller))
        {
            if (!transitioning)
            {
                transitioning = true;
                LevelStartFade.Instance.FadeOut();
                StartCoroutine(GoToNextScene());

            }
        }
    }

    IEnumerator GoToNextScene()
    {
        yield return new WaitForSeconds(fadeLength);
        GameManager.Instance.GoToNextScene();
    }
}
