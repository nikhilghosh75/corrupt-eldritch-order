using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDroneMovementAction : EnemyMoveAction
{
    // Range of times drone should move
    [SerializeField] int minNumMoves = 3;
    [SerializeField] int maxNumMoves = 6;

    // Time it takes for drone to move to new position
    [SerializeField] float speed = 10f;

    // Min distance drone should be from player
    [SerializeField] float minDistanceFromPlayer = 5f;

    // Box radius for drone's movable area
    [SerializeField] float movableRadius = 10f;

    // Wait time between movements
    [SerializeField] float waitTime = 0.5f;

    // Track num of moves for time keeping purposes
    float moveTime;

    // Track action finished
    bool actionFinished;
    Coroutine moveCoroutine;
    GameObject roomObjectReference;

    public override void Act()
    {
        actionFinished = false;
        //Debug.Log("Drone Move action");
        
        // Select Coordinates to fly to and calculate flight time
        List<Vector3> flightPath = new();
        moveTime = 0;
        int numFlights = Mathf.FloorToInt(Random.Range(minNumMoves, maxNumMoves + 0.99999f));
        for (int i=0; i<numFlights; ++i)
        {
            Vector3 p = GetFlightCoordinate();

            if (i == 0)
            {
                moveTime += Vector3.Distance(transform.position, p) / speed;
            }
            else
            {
                moveTime += Vector3.Distance(flightPath[flightPath.Count-1], p) / speed;
            }

            moveTime += waitTime;

            flightPath.Add(p);
        }

        moveCoroutine = StartCoroutine(FlyPath(flightPath));
    }

    public override float GetActionTime()
    {
        return moveTime + 0.1f;
    }

    public override bool ActionFinished()
    {
        return actionFinished;
    }

    private IEnumerator FlyPath(List<Vector3> path)
    {
        foreach (Vector3 p in path)
        {
            GetComponent<Rigidbody2D>().velocity = (p - transform.position).normalized * speed;
            float travelTime = Vector3.Distance(p, transform.position) / speed;

            yield return new WaitForSeconds(travelTime);

            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            yield return new WaitForSeconds(waitTime);
        }

        actionFinished = true;
    }

    private Vector3 GetFlightCoordinate()
    {
        // Pick find max and min coordinates near player
        Vector3 playerPosition = PlayerController.Instance.transform.position;
        float xMin = playerPosition.x - movableRadius;
        float xMax = playerPosition.x + movableRadius;
        float yMin = playerPosition.y - movableRadius;
        float yMax = playerPosition.y + movableRadius;

        // Get Room bounds
        GameObject room = GetRoomObject();
        BoxCollider2D boxCollider = room.GetComponent<BoxCollider2D>();
        float roomXMax = boxCollider.bounds.center.x + boxCollider.bounds.extents.x;
        float roomXMin = boxCollider.bounds.center.x - boxCollider.bounds.extents.x;
        float roomYMax = boxCollider.bounds.center.y + boxCollider.bounds.extents.y;
        float roomYMin = boxCollider.bounds.center.y - boxCollider.bounds.extents.y;

        // Adjust the box based on the boundary of the room
        xMin = Mathf.Max(xMin, roomXMin);
        xMax = Mathf.Min(xMax, roomXMax);
        yMin = Mathf.Max(yMin, roomYMin);
        yMax = Mathf.Min(yMax, roomYMax);

        // If the box is impossible, return the current position
        if (xMin > xMax || yMin > yMax)
        {
            return transform.position;
        }

        // Choose random position that isn't near the player
        // Only randomly pick a max of 5 times to avoid stalling
        Vector3 newPosition = new();
        for (int i=0; i<5; ++i)
        {
            newPosition = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), 0f);
            if (Vector3.Distance(transform.position, newPosition) > minDistanceFromPlayer)
            {
                break;
            }
        }

        return newPosition;
    }

    public override void Interrupt()
    {
        StopCoroutine(moveCoroutine);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    // Returns a reference to the room the drone is in
    private GameObject GetRoomObject()
    {
        // If room has already been assigned return it
        if (roomObjectReference)
        {
            return roomObjectReference;
        }

        // Check the parent object if its a level
        if (transform.parent && transform.parent.CompareTag("Level"))
        {
            return roomObjectReference = transform.parent.gameObject;
        }

        // If all else fails, find the closest level object
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Level");
        float distance = 99999f;
        GameObject closestRoom = null;
        foreach (GameObject room in rooms)
        {
            float newDistance = Vector3.Distance(room.transform.position, transform.position);
            if (newDistance < distance)
            {
                closestRoom = room;
                distance = newDistance;
            }
        }

        return roomObjectReference = closestRoom;
    }
}
