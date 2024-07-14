using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAbovePlayerAction : EnemyMoveAction
{
    [SerializeField] float distanceAbovePlayer;
    [SerializeField] float speed = 10f;

    Transform player;

    float flightTime;

    bool actionFinished = false;
    Coroutine moveCoroutine;

    private void Start()
    {
        player = PlayerController.Instance.transform;
    }

    public override void Act()
    {
        actionFinished = false;
        Vector3 destination = player.position + Vector3.up * distanceAbovePlayer;
        flightTime = Vector3.Distance(transform.position, destination) / speed;
        moveCoroutine = StartCoroutine(MoveAbovePlayer());
    }

    IEnumerator MoveAbovePlayer()
    {
        Vector3 destination = player.position + Vector3.up * distanceAbovePlayer;

        while (Vector3.Distance(transform.position, destination) > 0.1f)
        {
            GetComponent<Rigidbody2D>().velocity = (destination - transform.position).normalized * speed;
            destination = player.position + Vector3.up * distanceAbovePlayer;
          
            yield return null;
        }
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        actionFinished = true;
    }

    public override float GetActionTime()
    {
        return flightTime + 0.1f;
    }

    public override bool ActionFinished()
    {
        return actionFinished;
    }

    public override void Interrupt()
    {
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        StopCoroutine(moveCoroutine);
    }
}
