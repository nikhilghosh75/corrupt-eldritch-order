using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using Unity.Mathematics;


[System.Serializable]
public class PlayerWallController : PlayerSubController
{
    Collider2D wall;

    public float maxSlideSpeed;
    public float wallJumpHorizontalVelocity;
    public float wallJumpVerticalVelocity;
    public float wallJumpInputDelay = 0.1f;
    public bool isWallOnRight = false;

    float currentSlideSpeed;
    

    public override void Initialize()
    {
        name = "Wall Controller";
        playerController.Character_Anim.SetValue(false, false, true);
    }

    public override void OnDisable()
    {
        
    }

    public override void OnEnable()
    {
        wall = Physics2D.OverlapCircle(playerController.wallCheck.position, 0.2f, playerController.wallLayer);
        if (wall == null)
        {
            wall = Physics2D.OverlapCircle(playerController.wallCheckBehind.position, 0.2f, playerController.wallLayer);
        }
        // playerController.Character_Anim.TriggerLandWall(); <- caused player sprite to face wrong direction while on wall
        // needs to be removed or fixed
        isWallOnRight = wall.transform.position.x - playerController.transform.position.x > 0;
        currentSlideSpeed = 0;
    }

    public override void RecieveInput(PlayerInputType type)
    {
        switch (type)
        {
            case PlayerInputType.Jump:
                PerformWallJump();
                break;
        }
    }

    public override void FixedUpdate()
    {
        bool isOnGround = Physics2D.OverlapCircle(playerController.groundCheck.position, 0.2f, playerController.groundLayer);

        if (isOnGround)
        {
            playerController.airController.ResetJumps();
            playerController.SetController(playerController.groundController);
        }
        else
        {
            if (isWallOnRight)
            {
                playerController.transform.localScale =
                    new Vector2(-math.abs(playerController.transform.localScale.x), playerController.transform.localScale.y);
            }
            else
            {
                playerController.transform.localScale =
                    new Vector2(math.abs(playerController.transform.localScale.x), playerController.transform.localScale.y);
            }

            bool isOnWall = Physics2D.OverlapCircle(playerController.wallCheck.position, 0.2f, playerController.wallLayer);
            if (!isOnWall)
            {
                isOnWall = Physics2D.OverlapCircle(playerController.wallCheckBehind.position, 0.2f, playerController.wallLayer);
            }
            if (!isOnWall)
            {
                playerController.SetControllerWithDelay(playerController.airController, wallJumpInputDelay);
                playerController.airController.fallingOffLedge = true;
            }
            else
            {
                if (playerController.rb.velocity.y < -playerController.wallController.maxSlideSpeed)
                {
                    playerController.rb.velocity = new Vector2(playerController.horizontal, -playerController.wallController.maxSlideSpeed);
                }
                playerController.airController.ResetJumps();
            }
        }
    }

    public override void Move(InputAction.CallbackContext context)
    {
        playerController.horizontal = Mathf.Round(context.ReadValue<Vector2>().x);
        isWallOnRight = wall.transform.position.x - playerController.transform.position.x > 0;

        // Detach if holding away from wall
        if((isWallOnRight && playerController.horizontal < 0) || (!isWallOnRight && playerController.horizontal > 0))
            playerController.SetController(playerController.airController);
    }

    void PerformWallJump()
    {
        wall = Physics2D.OverlapCircle(playerController.wallCheck.position, 0.2f, playerController.wallLayer);
        if (wall == null)
        {
            wall = Physics2D.OverlapCircle(playerController.wallCheckBehind.position, 0.2f, playerController.wallLayer);
        }
        if (wall == null)
        {
            return;
        }
        isWallOnRight = wall.transform.position.x - playerController.transform.position.x > 0;
        float horizontalJumpVelocity = isWallOnRight ? -wallJumpHorizontalVelocity : wallJumpHorizontalVelocity;
        playerController.rb.velocity = new Vector2(horizontalJumpVelocity, wallJumpVerticalVelocity);
        playerController.airController.ResetJumps();
        playerController.horizontal = (isWallOnRight ? -1f : 1f);
        playerController.SetController(playerController.airController);
    }

    
}
