using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNearPlayerAction : EnemyMoveAction
{
    [SerializeField] float minRadians = 0.2f;
    [SerializeField] float maxRadians = 0.8f;
    [SerializeField] float minDistance = 3f;
    [SerializeField] float maxDistance = 5f;

    [SerializeField] float speed = 10f;

    PlayerHealth playerHealth;

    bool actionFinished;
    Coroutine moveCoroutine;

    private void Start()
    {
        playerHealth = GameObject.FindObjectOfType<PlayerHealth>();
    }

    float actionTime;

    public override void Act()
    {
        actionFinished = false;

        float radians = Random.Range(minRadians, maxRadians);
        float distance = Random.Range(minDistance, maxDistance);

        float xOffset = Mathf.Cos(radians * Mathf.PI) * distance;
        float yOffset = Mathf.Sin(radians * Mathf.PI) * distance;

        Vector3 destination = playerHealth.transform.position + new Vector3(xOffset, yOffset, 0);

        actionTime = Vector3.Distance(destination, transform.position) / speed;
        moveCoroutine = StartCoroutine(MoveTowardPosition(destination));
    }

    IEnumerator MoveTowardPosition(Vector3 destination)
    {
        GetComponent<Rigidbody2D>().velocity = (destination - transform.position).normalized * speed;
        yield return new WaitForSeconds(actionTime);
        actionFinished = true;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    public override float GetActionTime()
    {
        return actionTime;
    }

    public override bool ActionFinished()
    {
        return actionFinished;
    }

    public override void Interrupt()
    {
        StopCoroutine(moveCoroutine);
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
}
