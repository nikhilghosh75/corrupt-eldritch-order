using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehavior : MonoBehaviour
{
    protected Coroutine currentRoutine = null;
    EnemyAction currentAction = null;

    public bool activateOnVisible = true;

    [Header("Freezing")]
    public Material frozenMaterial;
    public bool stopVelocityOnFreeze;
    bool isFreezing = false;

    public EnemyAction CurrentAction => currentAction;

    private void OnBecameVisible()
    {
        if (activateOnVisible)
        {
            if (currentRoutine == null)
                currentRoutine = StartCoroutine(EnemyRoutine());
        }
    }

    public void Activate()
    {
        if (currentRoutine == null)
            currentRoutine = StartCoroutine(EnemyRoutine());
    }

    // The enemy loop coroutine which will execute enemy's actions for duration of it's life
    protected abstract IEnumerator EnemyRoutine();

    protected virtual void OnFreeze() { }

    // Function used to execute an action for set period of time
    protected IEnumerator ExecuteAction(EnemyAction action)
    {
        currentAction = action;
        action.Act();
        // Debug.Log($"Wait time: {action.GetActionTime()}");
        yield return new WaitForSeconds(action.GetActionTime());
        currentAction = null;
    }

    // Function used to execute action until completion
    protected IEnumerator ExecuteActionUntilFinished(EnemyAction action)
    {
        currentAction = action;
        action.Act();

        // Locks coroutine until action is finished
        while (!action.ActionFinished())
        {
            yield return null;
        }

        currentAction = null;
    }

    public void Freeze(float duration)
    {
        StartCoroutine(PerformFreeze(duration));
    }

    IEnumerator PerformFreeze(float duration)
    {
        if (isFreezing)
        {
            yield break;
        }

        isFreezing = true;
        EventBus.Publish(new EnemyFrozenEvent());

        if (currentAction != null)
        {
            currentAction.Interrupt();
            currentAction = null;
        }

        if (stopVelocityOnFreeze)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Material originalMaterial = null;
        if (spriteRenderer)
        {
            originalMaterial = spriteRenderer.material;
            spriteRenderer.material = frozenMaterial;
        }



        yield return new WaitForSeconds(duration);

        currentRoutine = StartCoroutine(EnemyRoutine());
        if (spriteRenderer)
        {
            spriteRenderer.material = originalMaterial;
        }
        isFreezing = false;
    }    
}
