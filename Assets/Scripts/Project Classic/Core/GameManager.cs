using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool useSeed;
    public int seed;
    public List<string> scenes;
    public int currentSceneIndex;

    bool isLoadingScene = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void StartRun()
    {
        RunManager.Instance.ResetRun();

        currentSceneIndex = 0;
        SceneManager.LoadScene(scenes[currentSceneIndex]);
    }

    public void GoToNextScene()
    {
        if (isLoadingScene)
        {
            return;
        }

        currentSceneIndex++;
        if (currentSceneIndex < scenes.Count)
        {
            RunManager.Instance.Save();
            StartCoroutine(LoadSceneAndActivate(scenes[currentSceneIndex]));
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void GoToFirstLevel() 
    {
        if (isLoadingScene) 
        {
            return;
        }

        //Save permanent data but delete run data
        RunManager.Instance.SavePermanentProgress();
        RunManager.Instance.ResetRun();

        currentSceneIndex = 0;
        StartCoroutine(LoadSceneAndActivate(scenes[currentSceneIndex]));
    }

    IEnumerator LoadSceneAndActivate(string sceneName)
    {
        isLoadingScene = true;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = true;
        operation.completed += (operation) =>
        {
            isLoadingScene = false;
            RunManager.Instance.Load();
        };

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
