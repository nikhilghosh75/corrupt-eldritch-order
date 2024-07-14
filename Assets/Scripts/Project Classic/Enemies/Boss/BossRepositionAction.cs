using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRepositionAction : EnemyMoveAction
{
    [SerializeField] BossAnimationController animationController;

    // Controls how quickly boss moves to new position
    [SerializeField] public float maxVelocity = 5f;
    [SerializeField] public float acceleration = 5f;

    // min distance change and margin from room edge
    [SerializeField] float minDistanceToMove = 5f;
    [SerializeField] float horizontalEdgeMargin = 1.5f;
    [SerializeField] float verticalEdgeMargin = 6f;

    // Track boss's current position
    Vector3 currentPosition = Vector3.zero;

    // Tracks if movement is finished
    bool movementFinished;

    // Center transform
    public Transform center;

    // Rigid body
    Rigidbody2D rigid;

    Coroutine moveCoroutine;

    public override void Act()
    {
        if (!rigid)
            rigid = GetComponent<Rigidbody2D>();

        movementFinished = false;

        moveCoroutine = StartCoroutine(MoveToNewPositionRoutine());
    }

    IEnumerator MoveToNewPositionRoutine()
    {
        // Grab room bounds
        BoxCollider2D roomCollider = GetComponent<BossBehavior>().GetRoomObject().GetComponents<BoxCollider2D>()[1];
        float maxX = roomCollider.bounds.center.x + (roomCollider.bounds.extents.x - horizontalEdgeMargin);
        float minX = roomCollider.bounds.center.x - (roomCollider.bounds.extents.x - horizontalEdgeMargin);
        float maxY = roomCollider.bounds.center.y + (roomCollider.bounds.extents.y - verticalEdgeMargin);
        float minY = roomCollider.bounds.center.y - (roomCollider.bounds.extents.y - verticalEdgeMargin);

        if (currentPosition == Vector3.zero)
        {
            currentPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
        }
        else
        {
            int numDraws = 0;
            Vector3 newPosition;
            do
            {
                // Pick a new position
                newPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);

                // Iterate num draws
                numDraws++;
            } while (Vector3.Distance(currentPosition, newPosition) < minDistanceToMove && numDraws < 5);
            currentPosition = newPosition;
        }

        animationController.Moving = true;
        
        while (true)
        {
            Vector3 targetPosition = GetTargetPosition();
            // If target is reached, exit the loop
            if (Vector3.Distance(targetPosition, transform.position) < 1f)
            {
                break;
            }

            // Accelerate toward the target position
            Vector3 directionVector = (targetPosition - transform.position).normalized;
            Vector3 currentVelocity = rigid.velocity;
            currentVelocity += acceleration * directionVector * 0.1f;

            // If exceeded max velocity, cap the velocity
            if (currentVelocity.magnitude > maxVelocity)
            {
                currentVelocity = maxVelocity * (currentVelocity / currentVelocity.magnitude);
            }

            rigid.velocity = currentVelocity;

            yield return new WaitForSeconds(0.1f);
        }

        if (!center)
        {
            center = FindObjectOfType<BossTrigger>().bossRoomCenter.transform;
        }
        GetComponent<BossBehavior>().currentRelativePosition = transform.position - center.position;
        movementFinished = true;
        animationController.Moving = false;
    }

    Vector3 GetTargetPosition()
    {
        return currentPosition;
    }

    public override float GetActionTime()
    {
        return 0f;
    }

    public override bool ActionFinished()
    {
        return movementFinished;
    }

    public override void Interrupt()
    {
        StopCoroutine(moveCoroutine);
    }
}
