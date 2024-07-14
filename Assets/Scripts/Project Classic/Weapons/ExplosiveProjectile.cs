using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : Projectile
{
    public float explosionRadius;

    public GameObject explosionPrefab;

    // Add any specific properties or methods for ExplosiveProjectile here

    // This is annoying but it allows there to be multiple explosive projectiles without breaking anyone else's code
    public List<GameObject> explosivePrefabs = new();
    public List<float> explosiveRadii = new();

    private void Start()
    {
        onDeath.AddListener(Explode);
    }

    // Override the Destroy method if needed
    public override void Destroy()
    {
        // Custom logic for explosive projectile destruction
        // For example, trigger an explosion or apply damage in a radius
        if (explosionPrefab)
            explosionPrefab.transform.gameObject.transform.localScale = new Vector3(explosionRadius, explosionRadius, 1);
        for (int i=0; i<explosivePrefabs.Count; ++i)
        {
            explosivePrefabs[i].transform.gameObject.transform.localScale = new Vector3(explosiveRadii[i], explosiveRadii[i], 1);
        }  
        Explode();
        base.Destroy();
    }

    void Explode()
    {
        // Custom explosion logic
        if (explosionPrefab)
        {
            GameObject.Instantiate(explosionPrefab, transform.position, Quaternion.identity).GetComponent<WSoft.Combat.Explode>().damage = damage; //set explosion to do same damage as base projectile
        }
        foreach (GameObject explosive in explosivePrefabs)
        {
            GameObject.Instantiate(explosive, transform.position, Quaternion.identity);
        }
    }
}