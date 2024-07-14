using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameOverScreen : MonoBehaviour
{
    public GameOverFeatContainer featContainer;
    public GameOverUnlocks unlocks;
    public GameOverKilledBy killedBy;
    public GameOverProgress progress;
    public GameOverEnemiesKilled enemiesKilled;

    // Start is called before the first frame update
    void Start()
    {
        EventBus.Subscribe<UpdateHealthEvent>(OnHealthChanged);
    }

    void OnHealthChanged(UpdateHealthEvent e)
    {
        if (e.health + e.healthDelta <= 0)
        {
            PlayerController.Instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
            RunManager.Instance.SavePermanentProgress();
            StartCoroutine(PerformGameOverScreen());
        }
    }

    IEnumerator PerformGameOverScreen()
    {
        unlocks.ResetUnlockDisplays();
        killedBy.ResetKilledBy();
        enemiesKilled.ResetEnemiesKilled();

        yield return StartCoroutine(featContainer.ShowFeats());
        yield return StartCoroutine(unlocks.ShowUnlocks());
        yield return StartCoroutine(killedBy.ShowKilledBy());
        yield return StartCoroutine(progress.ShowProgress());
        yield return StartCoroutine(enemiesKilled.ShowEnemiesKilled());
    }
}
