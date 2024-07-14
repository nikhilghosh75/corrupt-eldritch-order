using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMoveAction : EnemyAction
{
    // Helper movement function which moves the game object to a destination over a set period of time
    protected IEnumerator LerpTowardDestination(Vector3 destination, float duration)
    {
        Vector3 startPosition = transform.position;
        float startTime = Time.time;

        float progress = 0f;

        while (progress < 1)
        {
            progress = (Time.time - startTime) / duration;

            transform.position = Vector3.Lerp(startPosition, destination, progress);

            // Wait 3 frames
            for (int i = 0; i < 3; i++)
            {
                yield return null;
            }
        }

        transform.position = destination;
    }
}
