using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebufferEnemy : BaseJumpingEnemy
{
    //TODO: should enemy collide and knockback player even if not charging?

    [Header("Charge Attack")]
    public float chargeDuration = 1.0f;
    public float chargeRange = 10.0f;
    public float chargeSpeedMultiplier = 5.0f;
    public float chargeDelay = 1.5f;
    public float chargeCooldown = 1.0f;
    public float chargeShakeSpeed = 50f;
    public float chargeShakeAmount = 0.1f;

    DebufferChargeAction chargeAction;

    [SerializeField]
    GameObject target;

    [SerializeField]
    Animator anim;

    private void Start()
    {
        base.Start();

        chargeAction = GetComponent<DebufferChargeAction>();
    }

    protected override IEnumerator EnemyRoutine()
    {
        anim = GetComponent<Animator>();
        while (true)
        {
            UpdateOrientation();
            if (canMove)
            {
                Move();
                if (canAct)
                {
                    if (DistanceToPlayerY() > 1 && IsPlayerGrounded())
                    {
                        yield return StartCoroutine(Jump());
                    }
                    else if (Mathf.Abs(DistanceToPlayerX()) < chargeRange && DistanceToPlayerY() > -1)
                    {
                        yield return ExecuteAction(chargeAction);
                        
                    }
                }

            }
            yield return null;
        }
    }

    public void Death()
    {
        canMove = false;
        canAct = false;
        StartCoroutine(DeathAnim());
    }

    IEnumerator DeathAnim()
    {
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(1.65f);
        Destroy(this.gameObject);
    }
}
