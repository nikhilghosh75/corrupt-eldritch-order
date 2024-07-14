using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[System.Serializable]
public class PlayerAirController : PlayerSubController
{
    [Tooltip("Maximum horizontal airspeed.")] public float horizontalAirSpeed;
    [Tooltip("Initial jump velocity")] public float jumpingPower;
    public int maxExtraJumps;
    public int jumpsRemaining;
    [Tooltip("Percent of maximum speed gained per frame of input")] public float airAccelFactor;
    [Tooltip("Percent of current speed lost per frame of no input")] public float airDecelFactor;
    public int framesBeforeGroundCheck;

    public bool fallingOffLedge = false;
    public int coyoteTimeFrames;
    public int jumpVelocityAffectsFrames;

    int framesSinceAirborne = 0;

    public float maxFallVelocity = 60f;

    public override void Initialize()
    {
        jumpsRemaining = maxExtraJumps;
        name = "Air Controller";
        
    }

    public override void OnDisable()
    {

    }

    public override void OnEnable()
    {
        framesSinceAirborne = 0;
        playerController.Character_Anim.SetValue(false, false, false);
    }

    public override void FixedUpdate()
    {
        framesSinceAirborne++;
        
        // The direction of the jump should be the only direction considered for the
        // first few frames of the jump, otherwise the jump will
        if (framesSinceAirborne > jumpVelocityAffectsFrames)
        {
            playerController.horizontal = Mathf.Round(playerController.moveAction.action.ReadValue<Vector2>().x);
        }

        if (playerController.horizontal != 0)
        {   
            playerController.rb.velocity = new Vector2(playerController.rb.velocity.x + (playerController.horizontal * horizontalAirSpeed * airAccelFactor), playerController.rb.velocity.y);
            if (Mathf.Abs(playerController.rb.velocity.x) > horizontalAirSpeed)
            {
                playerController.rb.velocity = new Vector2(playerController.rb.velocity.x * airDecelFactor, playerController.rb.velocity.y);    
            }

        }
        else
        {
            playerController.rb.velocity = new Vector2(playerController.rb.velocity.x * airDecelFactor, playerController.rb.velocity.y);
        }

        // If we've fallen off a ledge and its past Coyote Time, we need to remove a jump
        if (fallingOffLedge)
        {
            if (framesSinceAirborne >= coyoteTimeFrames)
            {
                jumpsRemaining--;
                fallingOffLedge = false;
            }
        }

        if (framesSinceAirborne > framesBeforeGroundCheck)
        {
            if (Physics2D.OverlapCircle(playerController.groundCheck.position, 0.2f, playerController.groundLayer))
            {
                ResetJumps();
                playerController.SetController(playerController.groundController);
            }
        }

        if (Physics2D.OverlapCircle(playerController.wallCheck.position, 0.2f, playerController.wallLayer))
        {
            if (!Physics2D.OverlapCircle(playerController.wallCheck.position, 0.2f, playerController.wallLayer).name.Contains("Platform"))
            {
                // account for thin platforms that player can jump through
                // causes issues if player can walljump off these
                playerController.SetController(playerController.wallController);
            }
        }

        if (Physics2D.OverlapCircle(playerController.wallCheckBehind.position, 0.2f, playerController.wallLayer))
        {
            if (!Physics2D.OverlapCircle(playerController.wallCheckBehind.position, 0.2f, playerController.wallLayer).name.Contains("Platform"))
            {
                // account for thin platforms that player can jump through
                // causes issues if player can walljump off these
                playerController.SetController(playerController.wallController);
            }
        }

        if (IsOnCeiling())
        {
            ResetJumps();
            playerController.SetController(playerController.groundController);
            playerController.groundController.inTransitionFromAirToCeiling = true;
            playerController.groundController.SetOnCeiling();
        }

        if (playerController.rb.velocity.y < -maxFallVelocity) // Set max fall speed
            playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, - maxFallVelocity);
    }

    public override void Move(InputAction.CallbackContext context)
    {
        playerController.horizontal = Mathf.Round(context.ReadValue<Vector2>().x);
        if (Mathf.Round(context.ReadValue<Vector2>().y) == -1)
            playerController.groundController.isHoldingDown = true;
        else
            playerController.groundController.isHoldingDown = false;
    }

    public void Jump()
    {
        if (jumpsRemaining > 0)
        {
            playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, jumpingPower);
            jumpsRemaining--;
            playerController.Character_Anim.TriggerJump();
            EventBus.Publish(new JumpEvent());
        }
        else
            playerController.SetController(playerController.groundController);

    }

    public void ExtraJump()
    {
        if (jumpsRemaining > 0)
        {
            playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, jumpingPower);
            jumpsRemaining--;

            EventBus.Publish(new JumpEvent());
        }
    }

    public void DownwardJump()
    {
        playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, -jumpingPower);
        jumpsRemaining--;
    }

    public void ResetJumps()
    {
        jumpsRemaining = maxExtraJumps;
    }

    public override void RecieveInput(PlayerInputType type)
    {
        switch (type)
        {
            case PlayerInputType.Dash:
                playerController.SetController(playerController.dashController);
                return;
            case PlayerInputType.Jump:
                ExtraJump();
                return;
        }
    }

    bool IsOnCeiling()
    {
        Collider2D col = Physics2D.OverlapCircle(playerController.ceilingCheck.position, 0.2f, playerController.groundLayer);

        if (col == null) return false;
        return col.GetComponent<MagneticCeiling>() != null;
    }
    
    public void ChangeJumpStrength(float multiplier)
    {
        jumpingPower *= multiplier;
    }
}
