using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockProjectile : Projectile
{
    public float speed;
    Rigidbody2D rigid;

    [SerializeField]
    GameObject target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!target)
        {
            return;
        }
        Vector2 direction = target.transform.position - transform.position;
        rigid.velocity = (rigid.velocity.normalized + (direction.normalized/160)) * speed;
    }
}
