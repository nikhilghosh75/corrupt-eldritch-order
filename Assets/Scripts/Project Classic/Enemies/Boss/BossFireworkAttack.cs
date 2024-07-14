using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireworkAttack : EnemyFireProjectileAction
{
    [SerializeField] BossAnimationController animationController;
    [SerializeField] int numFireworks = 4;
    [SerializeField] float attackDelay = 2f;
    // how far from edge of room firework can be spawned
    [SerializeField] float roomBoundary = 8f;

    Coroutine routine;

    public override void Act()
    {
        GameObject room = GetComponent<BossBehavior>().GetRoomObject();
        BoxCollider2D collider = room.GetComponents<BoxCollider2D>()[1];

        float xRadius = collider.bounds.extents.x - roomBoundary;
        float yRadius = collider.bounds.extents.y - roomBoundary;
        Vector3 center = collider.bounds.center;

        animationController.Roar = true;

        // Select 4 random positions
        List<Vector3> targetPositions = new();
        for (int i = 0; i < numFireworks; ++i)
        {
            float targetX = Random.Range(center.x - xRadius, center.x + xRadius);
            float targetY = Random.Range(center.y - yRadius, center.y + yRadius);

            targetPositions.Add(new Vector3(targetX, targetY, 0));
        }

        routine = StartCoroutine(AttackRoutine(targetPositions));
    }

    IEnumerator AttackRoutine(List<Vector3> targetPositions)
    {
        foreach (Vector3 position in targetPositions)
        {
            GameObject fireworkInstance = GameObject.Instantiate(projectilePrefab, position, Quaternion.identity);

            fireworkInstance.GetComponent<Firework>().projectileData = projectileData;

            yield return new WaitForSeconds(attackDelay);
        }
    }

    public override float GetActionTime()
    {
        return numFireworks * attackDelay + cooldownTime;
    }

    public override void Interrupt()
    {
        StopCoroutine(routine);
    }

    private void OnDestroy()
    {
        Firework[] fireworks = FindObjectsOfType<Firework>();
        foreach (Firework firework in fireworks)
        {
            Destroy(firework.gameObject);
        }
    }
}
