using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using WSoft.Audio;
using WSoft.Combat;

public class MeleeSoldierEnemy : BaseJumpingEnemy
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
    public AudioEvent chargeSfx;

    [Header("Jumping")]
    /*public float jumpDuration = 2.0f;
    public float jumpDelay = 1.5f;
    public float jumpCooldown = 1.0f;*/
    public AudioEvent jumpSfx;
    public AudioEvent cooldownSfx;

    [Header("Knockback")]
    /*public float knockbackDuration = 0.25f;
    public Vector2 knockbackDirection;
    public float knockbackForce = 30.0f;
    public int knockbackDamage = 5;*/

    [SerializeField]
    GameObject target;

    [SerializeField]
    Animator anim;

    Coroutine actionCoroutine;

    protected override IEnumerator EnemyRoutine()
    {
        while (true)
        {
            if (healthData.isDying)
            {
                yield return null;
            }

            UpdateOrientation();
            if (canMove && enemyManager.playerInLevel && !frozen)
            {
                Move();
                if (canAct)
                {
                    if (DistanceToPlayerY() > 2 && IsPlayerGrounded())
                    {
                        actionCoroutine = StartCoroutine(Jump());
                        yield return actionCoroutine;
                        actionCoroutine = null;
                    }
                    else if (Mathf.Abs(DistanceToPlayerX()) < chargeRange && DistanceToPlayerY() > -1 && IsPlayerGrounded())
                    {
                        actionCoroutine = StartCoroutine(ChargeAttack());
                        yield return actionCoroutine;
                        actionCoroutine = null;
                    }
                }

            }
            yield return null;
        }
    }

    IEnumerator ChargeAttack()
    {
        canMove = false;
        canAct = false;

        //set attack direction before charging attack
        int chargeDirection = DistanceToPlayerX() > 0 ? 1 : -1;

        //dont move for a few seconds
        StartCoroutine(chargeAnimation());
        yield return new WaitForSeconds(chargeDelay);
        //play audio clip before charge? (TODO: ask design about implementation of charge sfx)
        chargeSfx.PlayAudio(gameObject);

        float timer = 0.0f;
        anim.SetTrigger("Attack");

        //charge in the same direction for duration
        while (timer < chargeDuration)
        {       
            Vector2 velocity = rb.velocity;
            velocity.x = chargeDirection * speed * chargeSpeedMultiplier;
            rb.velocity = velocity;

            //if collides with player during charge, apply knockback
            if (CheckPlayerCollision())
            {
                OnPlayerOverlap(chargeDirection);
                break;
            }
            else if(DistanceToPlayerX() / chargeDirection < 0)
            {
                break;
            }

            timer += Time.deltaTime;
            yield return null;
        }
        canMove = true;
        yield return new WaitForSeconds(chargeCooldown);
        anim.SetTrigger("Attack Done");
        canAct = true;
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

    protected override void OnFreeze()
    {
        base.OnFreeze();

        rb.velocity = Vector2.zero;
        
        if (actionCoroutine != null)
        {
            StopCoroutine(actionCoroutine);
        }
    }
}
