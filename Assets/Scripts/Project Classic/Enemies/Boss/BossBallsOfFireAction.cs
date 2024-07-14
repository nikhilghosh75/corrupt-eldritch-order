using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBallsOfFireAction : EnemyFireProjectileAction
{
    // Number of projectiles
    [SerializeField] int numProjectiles = 12;

    // Fire ball characteristics
    Vector3 center = Vector3.zero;
    [SerializeField] public float rotationSpeed;
    [SerializeField] float startingRadius;
    [SerializeField] float radiusChangeSpeed;
    [SerializeField] float projectileLifeTime;
    [SerializeField] float attackDuration;
    [SerializeField] float speedMultiplier = 5f;

    public override void Act()
    {
        if (center == Vector3.zero)
        {
            center = GetComponent<BossBehavior>().GetRoomObject().GetComponents<BoxCollider2D>()[1].bounds.center;
            startingRadius = GetComponent<BossBehavior>().GetRoomObject().GetComponents<BoxCollider2D>()[1].bounds.extents.x;
        }

        for (int i = 0; i < numProjectiles; ++i)
        {
            float radians = (float)i * (2f / numProjectiles);

            GameObject instance = GameObject.Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            instance.GetComponent<BallOfFire>().IntializeBallOfFire(center, radians, rotationSpeed, startingRadius, radiusChangeSpeed, projectileLifeTime, speedMultiplier);
        }
    }

    public override float GetActionTime()
    {
        return attackDuration;
    }

    public override void Interrupt()
    {
        
    }

    private void OnDestroy()
    {
        // Clear out all active balls of fire
        BallOfFire[] ballsOfFire = FindObjectsOfType<BallOfFire>();
        foreach (BallOfFire ballOfFire in ballsOfFire)
        {
            Destroy(ballOfFire.gameObject);
        }
    }
}
