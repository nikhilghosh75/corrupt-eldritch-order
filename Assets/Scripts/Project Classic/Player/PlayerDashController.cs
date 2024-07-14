using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class PlayerDashController : PlayerSubController
{
    private enum PlayerDashType
    {
        Dodgeroll,
        Dash
    }

    public float nextDashTime;
    public float dashSpeed;
    public float dodgeAngle = 0; //can potentially set based on the direction the player is holding in the future, would need additional code to handle. This value is in degrees. Affects Dodgeroll, not Dash
    public float dashCooldown;
    public float dashGravityScale;
    [SerializeField]
    private PlayerDashType dashtype = PlayerDashType.Dodgeroll;

    public float dashDuration;
    private float startDashTime;

    public bool airDodge = false;
    public bool attackDodge = false;

    public bool isDodging = false;

    PlayerInvincibilityController invincibilityController;
    float originalGravityScale;

    public override void Initialize()
    {
        name = "Dash Controller";
        invincibilityController = playerController.GetComponent<PlayerInvincibilityController>();
        originalGravityScale = playerController.rb.gravityScale;
    }

    public override void OnDisable()
    {
        playerController.rb.gravityScale = originalGravityScale;
    }

    public override void OnEnable()
    {
        if (Time.time >= nextDashTime)//only perform a dodge if it isn't on cooldown
        { 
            Dash();
            playerController.Character_Anim.TriggerDodge();
        }
    }

    public override void FixedUpdate()
    {
        if (Time.time - startDashTime > dashDuration)
        {
            isDodging = false;
            if (Physics2D.OverlapCircle(playerController.groundCheck.position, 0.2f, playerController.groundLayer))
                playerController.SetController(playerController.groundController);
            else
                playerController.SetController(playerController.airController);
        }
    }

    public override void Move(InputAction.CallbackContext context)
    {

    }

    public void Dash()
    {
        isDodging = true;
        EventBus.Publish(new DashEvent());
        startDashTime = Time.time;
        invincibilityController.ApplyInvincibility(dashCooldown);
        nextDashTime = Time.time + dashCooldown;
        playerController.rb.gravityScale = dashGravityScale;
        Vector2 dashDirection = playerController.actionAsset.currentActionMap.FindAction("Move").ReadValue<Vector2>().normalized;
        if (dashtype == PlayerDashType.Dodgeroll)
            playerController.rb.velocity = new Vector2(dashDirection.x * dashSpeed * Mathf.Cos(dodgeAngle * Mathf.Deg2Rad),
                dashDirection.y * dashSpeed * Mathf.Sin(dodgeAngle * Mathf.Deg2Rad)); //Angles dash with a consistent speed.
        else if (dashtype == PlayerDashType.Dash) //Old version of dash where vertical velocity is fully maintained. 
            playerController.rb.velocity = new Vector2(playerController.transform.localScale.x * dashSpeed, playerController.rb.velocity.y);
    }

    public override void RecieveInput(PlayerInputType type)
    {
        switch (type)
        {
            case PlayerInputType.Dash:
                playerController.SetController(playerController.dashController);
                return;
            case PlayerInputType.Jump:
                playerController.SetController(playerController.airController);
                playerController.airController.Jump();
                return;
        }
    }
}