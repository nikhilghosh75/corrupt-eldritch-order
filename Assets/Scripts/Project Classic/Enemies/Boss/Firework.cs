using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{
    // Projectile to spawn which triggers mode effects
    public ProjectileData projectileData;

    [SerializeField] float explosionDelay = 4f;
    [SerializeField] float scaleIncreaseSpeed = 1.2f;
    [SerializeField] GameObject explosionPrefab;

    // Store the start time to know when to explode
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale *= 1 + (scaleIncreaseSpeed * Time.deltaTime);
        if (Time.time - startTime > explosionDelay)
        {
            GameObject.Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
            if (projectileData)
            {
                // Spawn all explosions
                for (int i=0; i<projectileData.explosionPrefabs.Count; i++)
                {
                    GameObject explosionInstance = GameObject.Instantiate(projectileData.explosionPrefabs[i], transform.position, Quaternion.identity);
                    explosionInstance.transform.localScale = new Vector3(projectileData.explosionRadii[i], projectileData.explosionRadii[i], 1);
                }
            }
                
            Destroy(this.gameObject);
        }
    }
}
