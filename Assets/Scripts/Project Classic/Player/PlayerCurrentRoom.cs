using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrentRoom : MonoBehaviour
{
    public string currentRoomName { get; private set; }

    void Update()
    {
        DetectCurrentRoom();
    }

    void DetectCurrentRoom()
    {
        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);

        int layerMask = LayerMask.GetMask("Level Bound");

        Collider2D hitCollider = Physics2D.OverlapPoint(playerPosition, layerMask);

        if (hitCollider != null && hitCollider.CompareTag("Level"))
        {
            currentRoomName = hitCollider.gameObject.name;
        }
        else
        {
            currentRoomName = "No Room Detected";
        }
    }
}
