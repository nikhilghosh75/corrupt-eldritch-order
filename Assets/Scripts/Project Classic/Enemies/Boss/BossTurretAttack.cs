using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurretAttack : EnemyFireProjectileAction
{
    // Number of turrets to spawn
    [SerializeField] int numTurrets = 2;
    [SerializeField] float turretLifeTime = 5f;
    [SerializeField] float turretSpeed;

    List<GameObject> activeTurrets = new();

    Coroutine routine;

    public override void Act()
    {
        GameObject room = GetComponent<BossBehavior>().GetRoomObject();
        BoxCollider2D box = room.GetComponents<BoxCollider2D>()[1];

        // Calculate bounds
        Vector3 center = box.bounds.center;
        float xRadius = box.bounds.extents.x;
        float yRadius = box.bounds.extents.y;

        activeTurrets.Clear();
        for (int i=0; i<numTurrets; ++i)
        {
            // Spawn the turret
            GameObject turretInstance = GameObject.Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            turretInstance.SetActive(true);

            // Push instance into turret list
            activeTurrets.Add(turretInstance);

            // Initialize turret
            float xTarget = Random.Range(center.x - xRadius, center.x + xRadius);
            float yTarget = Random.Range(center.y - yRadius, center.y + yRadius);
            turretInstance.GetComponent<Turret>().InitializeTurret(projectileData, new Vector3(xTarget, yTarget, 0), 10);
        }

        routine = StartCoroutine(TurretCountdown());
    }

    IEnumerator TurretCountdown()
    {
        yield return new WaitForSeconds(turretLifeTime);

        foreach (GameObject turret in activeTurrets)
        {
            Destroy(turret);
        }
    }

    public override float GetActionTime()
    {
        return cooldownTime;
    }

    public override void Interrupt()
    {
        foreach (GameObject turret in activeTurrets)
        {
            Destroy(turret);
        }

        StopCoroutine(routine);
    }

    private void OnDestroy()
    {
        // Destroy all current turrets
        Turret[] turrets = FindObjectsOfType<Turret>();
        foreach (Turret turret in turrets)
        {
            Destroy(turret.gameObject);
        }
    }
}
