using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


[System.Serializable]
public class PlayerGroundController : PlayerSubController
{
    static readonly Vector3[] dropThroughPlatformOffsets = { new Vector3(0, -2, 0), new Vector3(1.625f, -2, 0), new Vector3(-1.625f, -2, 0) };

    public float groundSpeed;
    public float accelFactor;
    public float deccelFactor;

    float originalGroundSpeed;

    public bool inTransitionFromAirToCeiling = false;

    bool isOnCeiling = false;
    public bool inputReleasedInDash = false;
    public float maxVertVelocity = 35f;
    public bool isHoldingDown = false;

    float originalGravityScale;

    public override void Initialize()
    {
        name = "Ground Controller";
        originalGroundSpeed = groundSpeed;
        originalGravityScale = playerController.rb.gravityScale;
    }

    public override void OnDisable()
    {
        if (isOnCeiling)
        {
            Vector3 scale = playerController.transform.localScale;
            scale.y *= -1;
            playerController.transform.localScale = scale;

            playerController.rb.gravityScale *= -1f;
        }
    }

    public override void OnEnable()
    {
        playerController.airController.ResetJumps();
        playerController.Character_Anim.SetValue(true, playerController.horizontal != 0, false);
        playerController.Character_Anim.TriggerLandGround();
        isOnCeiling = false;
    }

    public override void FixedUpdate()
    {
        //Set player facing direction
        if (playerController.horizontal != 0 && !inputReleasedInDash)
        {
            if (playerController.horizontal != playerController.actionAsset.currentActionMap.FindAction("Move").ReadValue<Vector2>().x)
            {
                playerController.horizontal = playerController.actionAsset.currentActionMap.FindAction("Move").ReadValue<Vector2>().x;
            }

            Vector3 scale = playerController.transform.localScale;
            if (!playerController.aimLocked && !playerController.continuousAimingOn)
            {
                playerController.transform.localScale = new Vector3((playerController.horizontal < 0 ? Mathf.Abs(scale.x) * -1.0f : Mathf.Abs(scale.x) * 1.0f), scale.y, 1.0f);
            }
            // Set player velocity
            if (Mathf.Abs(playerController.rb.velocity.x) < groundSpeed || (playerController.rb.velocity.x * playerController.horizontal) < 0)
            {
                playerController.rb.velocity = new Vector2(playerController.rb.velocity.x + (playerController.horizontal * groundSpeed * accelFactor), playerController.rb.velocity.y);
            }
            // If the player is over the speed limit, decelerate
            if(Mathf.Abs(playerController.rb.velocity.x) > groundSpeed){
                playerController.rb.velocity = new Vector2(playerController.rb.velocity.x * deccelFactor, playerController.rb.velocity.y);
            }
        }

        else if (playerController.rb.velocity != Vector2.zero)
        {
            playerController.rb.velocity = new Vector2(playerController.rb.velocity.x * deccelFactor, playerController.rb.velocity.y);
        }

        // transition variable gives time for y scale and gravity scale to change before fixedupdate sends back to air controller
        if (!Physics2D.OverlapCircle(playerController.groundCheck.position, 0.2f, playerController.groundLayer) && !inTransitionFromAirToCeiling)
        {
            playerController.SetController(playerController.airController);

            if (!playerController.droppingThroughPlatform)
                playerController.airController.fallingOffLedge = true;
            else
                playerController.droppingThroughPlatform = false;
        }


        if (playerController.rb.velocity.x < -maxVertVelocity) // Should only be needed when dashing and not releasing input as ninja
            playerController.rb.velocity = new Vector2(-maxVertVelocity, playerController.rb.velocity.y);
        else if (playerController.rb.velocity.x > maxVertVelocity) // Should only be needed when dashing and not releasing input as ninja
            playerController.rb.velocity = new Vector2(maxVertVelocity, playerController.rb.velocity.y);
        playerController.Character_Anim.SetValue(true, playerController.horizontal != 0, false);
    }

    public override void Move(InputAction.CallbackContext context)
    {
        inputReleasedInDash = false;
        isHoldingDown = false;
        playerController.horizontal = Mathf.Round(context.ReadValue<Vector2>().x);
        playerController.vertical = Mathf.Round(context.ReadValue<Vector2>().y);
        return;
    }

    public override void RecieveInput(PlayerInputType type)
    {
        switch (type)
        {
            case PlayerInputType.Dash:
                playerController.SetController(playerController.dashController);
                return;
            case PlayerInputType.Jump:
                if (playerController.IsGrounded() && (playerController.vertical == -1 || isHoldingDown))
                {
                    // check for fall-throughable platforms
                    if (DropThroughPlatform())
                    {
                        playerController.SetController(playerController.airController);
                        return;
                    }
                    else if (isOnCeiling)
                    {
                        playerController.SetController(playerController.airController);
                        playerController.airController.DownwardJump();
                        return;
                    }
                    else
                    {
                        playerController.SetController(playerController.airController);
                        playerController.airController.Jump();
                        return;
                    }
                }
                else
                {
                    playerController.SetController(playerController.airController);
                    if (isOnCeiling)
                        playerController.airController.DownwardJump();
                    else
                        playerController.airController.Jump();
                    return;
                }
        }
    }

    public void ChangeSpeed(float multiplier) 
    { 
        groundSpeed *= multiplier;
    }

    public void SetOnCeiling()
    {
        isOnCeiling = true;

        Vector3 scale = playerController.transform.localScale;
        scale.y *= -1;
        playerController.transform.localScale = scale;

        playerController.rb.gravityScale = -1 * originalGravityScale;

        // variable gives time for y scale and gravity scale to change before fixedupdate sends back to air controller
        inTransitionFromAirToCeiling = false;
    }

    public float GroundSpeedMultiplier => groundSpeed / originalGroundSpeed;

    public bool DropThroughPlatform()
    {
        Vector3 position = playerController.transform.position;

        // Loop through all the positions to check
        foreach (Vector3 offset in dropThroughPlatformOffsets)
        {
            Vector3 castPosition = new Vector3(position.x + offset.x, position.y + offset.y, 0);
            RaycastHit2D cast = Physics2D.Raycast(castPosition, -playerController.transform.up, 2.0f, playerController.groundLayer);

            if (cast.collider != null)
            {
                PlatformEffector2D effect = cast.collider.GetComponent<PlatformEffector2D>();
                BoxCollider2D platformCollider = cast.collider.GetComponent<BoxCollider2D>();
                if (effect != null)
                {
                    // workaround to make sure enemies don't fall through the one-way platforms
                    // currently platform effectors aren't used except to check if the platform is an interior platform
                    Physics2D.IgnoreCollision(playerController.GetComponent<BoxCollider2D>(), platformCollider);
                    playerController.StartCoroutine(ResetPlatform(cast.collider.gameObject));
                    playerController.droppingThroughPlatform = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        return false;
    }

    IEnumerator ResetPlatform(GameObject platform)
    {
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerController.GetComponent<BoxCollider2D>(), platform.GetComponent<BoxCollider2D>(), false);
        yield return null;
    }
}
