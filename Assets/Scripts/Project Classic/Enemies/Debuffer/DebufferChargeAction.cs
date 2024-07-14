using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebufferChargeAction : EnemyAction
{
    public float chargeDuration = 1.0f;
    public float chargeRange = 10.0f;
    public float chargeSpeedMultiplier = 5.0f;
    public float chargeDelay = 1.5f;
    public float chargeCooldown = 1.0f;
    public float chargeShakeSpeed = 50f;
    public float chargeShakeAmount = 0.1f;

    GameObject target;
    Animator anim;
    Rigidbody2D rb;
    BaseJumpingEnemy jumpingEnemy;

    Coroutine chargeCoroutine;
    Coroutine animationCoroutine;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        jumpingEnemy = GetComponent<BaseJumpingEnemy>();
    }

    public override void Act()
    {
        target = PlayerController.Instance.gameObject;
        chargeCoroutine = StartCoroutine(ChargeAttack());
        animationCoroutine = StartCoroutine(chargeAnimation());
    }

    public override float GetActionTime()
    {
        return chargeDuration;
    }

    public override void Interrupt()
    {
        jumpingEnemy.canDamage = false;
        rb.velocity = new Vector2(0, 0);
    }

    IEnumerator ChargeAttack()
    {
        //set attack direction before charging attack
        int chargeDirection = DistanceToPlayerX() > 0 ? 1 : -1;
        jumpingEnemy.damageDirection = chargeDirection;
        //dont move for a few seconds
        yield return new WaitForSeconds(chargeDelay);

        float timer = 0.0f;
        //anim.SetTrigger("Attack");
        jumpingEnemy.canDamage = true;

        //charge in the same direction for duration
        while (timer < chargeDuration)
        {
            anim.SetTrigger("Attack");

            Vector2 velocity = rb.velocity;
            velocity.x = chargeDirection * jumpingEnemy.speed * chargeSpeedMultiplier;
            rb.velocity = velocity;

            if (DistanceToPlayerX() / chargeDirection < 0)
            {
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
        jumpingEnemy.canDamage = false;
        jumpingEnemy.EnableMovement();
        yield return new WaitForSeconds(chargeCooldown);
        jumpingEnemy.EnableActing();
    }

    IEnumerator chargeAnimation()
    {
        float timer = 0.0f;
        while (timer < chargeDelay)
        {

            Vector3 position = transform.position;

            position.x += Mathf.Sin(Time.time * chargeShakeSpeed) * chargeShakeAmount;

            transform.position = position;

            timer += Time.deltaTime;
            yield return null;
        }
    }

    float DistanceToPlayerX()
    {
        return target.transform.position.x - transform.position.x;
    }
}
