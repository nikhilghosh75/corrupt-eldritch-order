using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRainAction : EnemyFireProjectileAction
{
    // Track the current room the boss is in to determine where to spawn projectiles
    GameObject room;

    [SerializeField] BossAnimationController animationController;

    // duration to rain projectiles for
    [SerializeField] float actionTime = 3f;
    [SerializeField] float fireRate = 0.05f;

    private void Awake()
    {
        room = GetComponent<BossBehavior>().GetRoomObject();
    }

    public override void Act()
    {
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        float startTime = Time.time;

        // This will find the roof of the room and its coordinates to determine where it should fire projectiles from
        BoxCollider2D collider = room.GetComponents<BoxCollider2D>()[1];
        float xMin = collider.bounds.center.x - (collider.bounds.extents.x - 2);
        float xMax = collider.bounds.center.x + (collider.bounds.extents.x - 2);
        float ySpawnPosition = collider.bounds.center.y + (collider.bounds.extents.y - 2);

        animationController.Roar = true;

        // Fires projectiles for duration of action time
        while (Time.time - startTime < actionTime)
        {
            // Randomly instantiate the rain projectile in the room
            float xSpawnPosition = Random.Range(xMin, xMax);
            projectilePool.SpawnProjectile(projectileData, new Vector3(xSpawnPosition, ySpawnPosition, 0), Vector3.zero, Quaternion.identity);

            yield return new WaitForSeconds(fireRate);
        }

        yield return new WaitForSeconds(cooldownTime);
    }

    public override float GetActionTime()
    {
        return actionTime + cooldownTime;
    }

    public override void Interrupt()
    {

    }
}
