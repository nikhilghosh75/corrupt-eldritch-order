using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerKnockbackController : PlayerSubController
{
    float timer = 0;
    public float knockback_duration;
    public Vector2 knockback_direction;


    public IEnumerator ApplyKnockback(float maxDuration, Vector2 direction, float force)
    {
        if (timer != 0) yield break;
        //knockback_duration = duration;
        //Debug.Log(knockback_duration);
        knockback_direction = direction.normalized * force;

        //apply initial knockback force
        playerController.rb.velocity = Vector2.zero;
        playerController.rb.AddForce(knockback_direction, ForceMode2D.Impulse);
        /*Vector2 final = new Vector2(0, -10);

        while (timer < knockback_duration)
        {
            //damp knockback force over time for the duration
            playerController.rb.velocity = Vector2.Lerp(playerController.rb.velocity, final, timer / knockback_duration);

            timer += Time.deltaTime;
            if (playerController.IsGrounded() && timer > 0.5f)
            {
                break;
            }

            yield return null;
        }*/

        //stop any knockback force
        while (!playerController.IsGrounded() || Physics2D.OverlapCircle(playerController.wallCheck.position, 0.2f, playerController.wallLayer) ||
            Physics2D.OverlapCircle(playerController.wallCheckBehind.position, 0.2f, playerController.wallLayer) || timer < 0.5f)
        {
            yield return null;
            timer += Time.deltaTime;

            if (timer > maxDuration)
            {
                break;
            }
        }
        timer = 0;
        playerController.rb.velocity = Vector2.zero;

        //after knockback switch back to air or ground controller
        if (playerController.IsGrounded())
        {
            playerController.SetController(playerController.groundController);
        }
        else if (Physics2D.OverlapCircle(playerController.wallCheck.position, 0.2f, playerController.wallLayer) ||
            Physics2D.OverlapCircle(playerController.wallCheckBehind.position, 0.2f, playerController.wallLayer))
        {
            playerController.SetController(playerController.wallController);
        }
        else
        {
            playerController.SetController(playerController.airController);
        }
    }

    public override void Initialize()
    {
        name = "Knockback Controller";
    }

    public override void OnEnable()
    {
        
    }

    public override void OnDisable()
    {

    }

    public override void FixedUpdate()
    {
        //updates are handled in apply knockback function
    }

    public override void Move(InputAction.CallbackContext context)
    {
        //since player input will be locked during knockback, no need to process move
    }

    public override void RecieveInput(PlayerInputType type)
    {
        //do not accept input while the player is being knocked back
    }
}
