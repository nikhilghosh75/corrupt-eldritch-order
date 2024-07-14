using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBehavior : EnemyBehavior
{
    [SerializeField] LayerMask wallMask;

    // Activation Range
    [SerializeField] float activationRange = 20f;

    // Reference to behaviors
    [SerializeField] EnemyMoveAction droneMove;
    [SerializeField] EnemyMoveAction droneMoveNearPlayer;
    [SerializeField] EnemyFireProjectileAction enemyFireProjectile;

    // tells if drone is active
    bool droneActive = false;

    protected override IEnumerator EnemyRoutine()
    {
        while (true)
        {
            // Move
           yield return ExecuteActionUntilFinished(droneMove);

            // Move Near player if the player is attackable
            if (droneMoveNearPlayer)
                yield return ExecuteActionUntilFinished(droneMoveNearPlayer);
            else
                yield return new WaitForSeconds(1.5f);

            // Shoot
            yield return ExecuteAction(enemyFireProjectile);
        }
    }

    private void Update()
    {
        // Check every second if player is in range and enable or disable behavior appropriately
        if (Time.frameCount % 60 == 0)
        {
            Vector3 playerPosition = PlayerController.Instance.transform.position;
            if (Vector3.Distance(transform.position, playerPosition) < activationRange)
            {
                if (!droneActive)
                {
                    droneActive = true;
                    currentRoutine = StartCoroutine(EnemyRoutine());
                }
            }
            else
            {
                if (droneActive)
                {
                    droneActive = false;
                    StopCoroutine(currentRoutine);
                    currentRoutine = null;
                }
            }
        }       
    }

    // If there are walls between the player and the drone, the drone should not attack
    bool ShouldMoveNearPlayer()
    {
        Vector3 playerPosition = PlayerController.Instance.transform.position;

        RaycastHit2D hit = Physics2D.Linecast(transform.position, playerPosition, wallMask);
        return hit.collider == null;
    }
}
