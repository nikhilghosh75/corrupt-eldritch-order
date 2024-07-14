using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBombAction : EnemyFireProjectileAction
{
    Animator anim;

    private void Start()
    {
        projectilePool = ProjectilePool.Instance;
        anim = GetComponent<Animator>();
    }

    public override void Act()
    {
        projectilePool.SpawnProjectile(projectileData, firePosition.position, Vector3.down, Quaternion.identity);
        anim.SetTrigger("Shoot");
    }

    public override float GetActionTime()
    {
        return 1f;
    }

    public override void Interrupt()
    {
        
    }
}
