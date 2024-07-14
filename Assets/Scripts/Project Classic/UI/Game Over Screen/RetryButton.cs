using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetryButton : MonoBehaviour
{
    bool transitioning = false;
    public float fadeLength = 2.0f;

    public void RestartRun() 
    {
        if (!transitioning) 
        {
            transitioning = true;
            LevelStartFade.Instance.FadeOut();
            GameManager.Instance.GoToFirstLevel();
/*          Time.timeScale = 1f;
            StartCoroutine(LoadFirstLevel());*/
        }
    }

   /* IEnumerator LoadFirstLevel() 
    {
        yield return new WaitForSeconds(fadeLength);
        GameManager.Instance.GoToFirstLevel();
    }*/
}
