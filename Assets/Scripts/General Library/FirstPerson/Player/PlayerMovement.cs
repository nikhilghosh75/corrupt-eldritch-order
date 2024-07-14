/*
 * A movement script used in Bloom: A Tomb of Power. Intended for first person movement
 * REQUIRES: 1) The object its attached to must have a character controller. 2) The object must also have a PlayerInputData
 * Written by Alex Song '?, Natasha Badami '20, George Castle '22, Jacob Smellie '?, Matt Rader '19, Evan Brisita '18
 * 
 * This code may have aspects/assumptions that were specific to its original project. 
 * I would recommend using it as a reference (when implementing a new script), rather than purely copying it and pasting it
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WSoft.FirstPerson
{
    public class PlayerMovement : MonoBehaviour
    {
        //Variables
        public UnityEvent onJump;
        public float speed { get; set; } = 8.0F;
        public float dampTimeGroundAcceleration = .1f;
        public float dampTimeGroundDeceleration = .1f;
        public float dampTimeAirAcceleration = .3f;
        public float dampTimeAirDeceleration = .3f;
        public float jumpHeight = 3f;
        public float timeToJumpHeight = 1f;

        // Jump queue and Coyote Time
        public float secondsJumpQueueBuffer = .1f;
        public float secondsCoyoteTimeBuffer = .1f;

        // for jump pad
        public bool overrideJumpSpeed;
        public float jumpSpeedOverrideValue;

        // buffer for hitting ceiling detection
        public float secondsStuckToCeiling = 0.01f;

        // calculated helpers
        private float jumpSpeed;
        private float gravity;

        // actual velocity (with damping)
        private Vector3 moveVelocity = Vector3.zero;
        private float yVelocity;

        // variable for Vector3.SmoothDamp to track state
        // Do not modify
        private Vector3 dampAcceleration = Vector3.zero;

        // Moving Platforms
        private bool onMovingPlat = false;

        // Jump queue and Coyote Time
        private float secondsLastJumpCommand;
        private float secondsLastGround;

        // landing after jump/fall
        public float inAirThresholdTime;
        float lastTimestampGrounded;
        bool currentlyMoving;
        public UnityEvent onLand;
        public UnityEvent OnStartMoving;
        public UnityEvent OnStopMoving;

        // Jump slope max
        public float maxJumpSlopeHeight = 45;
        public float currentSlopeHeight = 0;

        private CharacterController cc;

        void Awake()
        {
            cc = GetComponent<CharacterController>();

            moveVelocity = Vector3.zero;
            secondsLastJumpCommand = -100f;
            secondsLastGround = -100f;

            gravity = 2 * jumpHeight / Mathf.Pow(timeToJumpHeight, 2);
            jumpSpeed = gravity * timeToJumpHeight;
        }

        void Update()
        {

            // calculate target velocity (move_velocity is real velocity with damping)
            //Feed moveDirection with input.
            Vector3 target_velocity = new Vector3(PlayerInputData.moveX, 0, PlayerInputData.moveZ);
            target_velocity = transform.TransformDirection(target_velocity);

            if (target_velocity != Vector3.zero && !currentlyMoving && isGrounded())
            {
                currentlyMoving = true;
                OnStartMoving.Invoke();
            }
            else if (target_velocity == Vector3.zero && currentlyMoving)
            {
                currentlyMoving = false;
                OnStopMoving.Invoke();
            }

            //normalize moveDirection so always same speed
            target_velocity = Vector3.Normalize(target_velocity);

            //Multiply it by speed.
            target_velocity *= speed;

            bool groundedTemp;
            if (groundedTemp = isGrounded())
            {
                // speeding up
                if (moveVelocity.sqrMagnitude < target_velocity.sqrMagnitude)
                {
                    moveVelocity = Vector3.SmoothDamp(moveVelocity, target_velocity, ref dampAcceleration, dampTimeGroundAcceleration);
                }
                else
                {
                    moveVelocity = Vector3.SmoothDamp(moveVelocity, target_velocity, ref dampAcceleration, dampTimeGroundDeceleration);
                }
                // determining if onLand should fire
                if (Time.time - lastTimestampGrounded > inAirThresholdTime)
                {
                    onLand?.Invoke();
                }
                lastTimestampGrounded = Time.time;
            }
            else
            {
                if (moveVelocity.sqrMagnitude < target_velocity.sqrMagnitude)
                {
                    moveVelocity = Vector3.SmoothDamp(moveVelocity, target_velocity, ref dampAcceleration, dampTimeAirAcceleration);
                }
                else
                {
                    moveVelocity = Vector3.SmoothDamp(moveVelocity, target_velocity, ref dampAcceleration, dampTimeAirDeceleration);
                }
            }

            // includes jumping
            SetYVelocity();

            float start = transform.position.y;

            //Making the character move
            // Check the move direction is not going up too large of a slope
            // Vector3 flatDirection = moveVelocity; flatDirection.y = 0;
            //if (IsOnValidSlope(flatDirection))
            //{
            cc.Move(moveVelocity * Time.deltaTime);
            if (onMovingPlat && yVelocity <= 0)
            {
                transform.position = new Vector3(transform.position.x, start, transform.position.z);
            }
            //}


            resetTrackingVariables();
        }

        bool jumpQueueJump()
        {
            // Track how many frames since last jump command
            if (PlayerInputData.isTryingJump)
            {
                secondsLastJumpCommand = Time.time;
            }

            // if there was a jump command in the buffer and player is grounded, jump
            if (Time.time - secondsLastJumpCommand <= secondsJumpQueueBuffer && isGrounded())
            {
                return true;
            }
            return false;
        }

        bool coyoteTimeJump()
        {
            if (isGrounded())
            {
                secondsLastGround = Time.time;
            }

            // if the player was grounded in buffer time and is trying to jump and is not already in a jump
            if (Time.time - secondsLastGround <= secondsCoyoteTimeBuffer && PlayerInputData.isTryingJump && yVelocity <= 0)
            {
                return true;
            }
            return false;
        }

        void resetTrackingVariables()
        {
            onMovingPlat = false;
        }

        bool isGrounded()
        {
            return (cc.isGrounded || onMovingPlat) && IsOnValidSlope();
        }

        void SetYVelocity()
        {

            moveVelocity.y = 0;

            // if the actual velocity is lower than the expected move speed, we collided with something that
            // stopped our jump (like the ceiling) and should just start falling
            if ((yVelocity - cc.velocity.y) / gravity >= secondsStuckToCeiling && cc.velocity.y == 0 && yVelocity > 0)
            {
                yVelocity = 0;
            }

            // if on ground or on moving platform, yVel = 0
            if (isGrounded())
                yVelocity = 0;
            // else if in air, apply gravity
            else
                yVelocity = yVelocity -= gravity * Time.deltaTime;

            //Jumping
            if (((isGrounded() && PlayerInputData.isTryingJump) || jumpQueueJump() || coyoteTimeJump()))
            {
                onJump?.Invoke();
                yVelocity = jumpSpeed;
            }

            // This occurs if jump speed is set externally
            if (overrideJumpSpeed)
            {
                yVelocity = jumpSpeedOverrideValue;
                overrideJumpSpeed = false;
            }

            moveVelocity.y = yVelocity;

        }

        public bool IsOnValidSlope()
        {
            return (Mathf.Abs(GetSlopeAngle()) < maxJumpSlopeHeight);
        }

        public float GetSlopeAngle()
        {
            RaycastHit hit;
            float angle = 0;
            if (Physics.SphereCast(this.transform.position, .5f, Vector3.down, out hit, 1, ~LayerMask.NameToLayer("Default")))
            {
                Vector3 collisionNormal = hit.normal;
                Vector3 hitDirection = (hit.point - this.transform.position); hitDirection.y = 0;
                angle = (Vector3.Angle(collisionNormal, hitDirection) - 90) % 90;
            }
            // Debug view value
            currentSlopeHeight = angle;
            return angle;
        }

        public void SetJumpSpeedOverride(float jumpSpeed_)
        {
            overrideJumpSpeed = true;
            jumpSpeedOverrideValue = jumpSpeed_;
        }

        public void SetOnMovingPlatform()
        {
            onMovingPlat = true;
        }
    }
}
