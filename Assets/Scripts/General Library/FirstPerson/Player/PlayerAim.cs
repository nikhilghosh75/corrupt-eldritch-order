/*
 * A script that controls a player's aim.
 * Requires: cinemachine package, a character controller on the object representing the player
 * Written by Natasha Badami '20, George Castle '22, Angela Salacata '?, Evan Brisita '18, Nico Williams '20, Yeonho Jang '?, Matthew Rader '19, Nigel Charleston '21
 * NOT APPROVED
 * 
 * This code may have aspects/assumptions that were specific to its original project. 
 * I would recommend using it as a reference (when implementing a new script), rather than purely copying it and pasting it
 */

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;

namespace WSoft.FirstPerson
{
    public class PlayerAim : MonoBehaviour
    {
        // GENERAL
        [Header("General")]
        [SerializeField] Settings settings;
        float mouseSensitivity;
        float xRotation = 0f;

        // CAMERA FOV
        [Header("Camera FOV")]
        private CinemachineVirtualCamera vcam;

        // CAMERA TILT
        [Header("Camera Tilt")]
        [SerializeField] float maxTilt = 20f;
        [SerializeField] CharacterController cc;
        float zRotation = 0f;


        // AIM ASSIST
        [Header("Aim Assist")]
        int enemyHurtboxLayer;
        [SerializeField] Transform Player;
        [SerializeField] LayerMask aimAssistLayerMask;
        bool didIntersectEnemy = false;
        [SerializeField] GameObject aimAssistCone;
        [SerializeField] RotateToTarget spellListRotateToTarget;
        GameObject currentTarget;

        private void Start()
        {
            // FOV
            vcam = GetComponent<CinemachineVirtualCamera>();
            vcam.m_Lens.FieldOfView = settings.FOV;

            // Add listener for settings values change
            Slider[] sliders = GameObject.Find("PauseMenu").GetComponentsInChildren<Slider>(true);
            foreach (Slider child in sliders)
            {
                if (child.name == "AimAssistStrength")
                {
                    child.onValueChanged.AddListener(UpdateAimAssistStrength);
                }

                else if (child.name == "FOV")
                {
                    child.onValueChanged.AddListener(UpdateFOV);
                }
            }

            // GENERAL
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            currentTarget = aimAssistCone;
            UpdateAimAssistStrength(settings.AimAssistStrength);
            UpdateFOV(settings.FOV);
            enemyHurtboxLayer = LayerMask.NameToLayer("Enemy Hurtbox");
            cc = Player.GetComponent<CharacterController>();
        }

        // ---------- AIM ASSIST ----------
        public void UpdateAimAssistStrength(float value /* unused */)
        {
            aimAssistCone.transform.localScale = new Vector3(settings.AimAssistStrength, settings.AimAssistStrength, aimAssistCone.transform.localScale.z);
        }

        void AimAssist()
        {
            // Compensate for the fact that on trigger exit doesn't run if triggering collider disabled/destroyed
            if (!currentTarget || currentTarget.activeSelf)
                currentTarget = aimAssistCone;

            // Check if raycast hits an enemy
            RaycastHit hit;
            didIntersectEnemy = Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, aimAssistLayerMask) &&
                        hit.collider.gameObject.layer == enemyHurtboxLayer;

            //Debug.Log(currentMouseSensitivity + " " + didIntersectEnemy);
            // If raycast intersects enemy and we just started looking at the enemy
            /*if (didIntersectEnemy && !isFacingEnemy)
            {
                currentMouseSensitivity = aimAssistSensitivityMultiplier * MouseSensitivity;
                isFacingEnemy = true;
            }
            else if (!didIntersectEnemy)
            {
                isFacingEnemy = false;
                currentMouseSensitivity = MouseSensitivity;
            }*/
        }

        public void ProcessEnemyInAimAssistTrigger(GameObject target)
        {
            // situation 1: target is the only enemy currently in the aim assist trigger, set currentTarget to target
            if (currentTarget == aimAssistCone)
            {
                currentTarget = target;
            }

            // situation 2: currentTarget is NOT aim assist cone, decide which target is closer, assign accordingly
            else
            {
                Vector3 position0 = currentTarget.transform.position;
                Vector3 position1 = target.transform.position;
                if (Vector3.Distance(position0, transform.position) > Vector3.Distance(position1, transform.position))
                {
                    currentTarget = target;
                }
            }
        }

        public void ProcessEnemyExitedAimAssistTrigger(GameObject target)
        {
            if (currentTarget == target)
            {
                currentTarget = aimAssistCone;
            }
        }

        public void RotateSpellListToTarget()
        {
            if (didIntersectEnemy || currentTarget == aimAssistCone)
                spellListRotateToTarget.FaceTargetWithVector3(aimAssistCone.transform.position);
            else
            {
                // using -forward because aim assist cone model faces the opposite direction of its positive z axis
                spellListRotateToTarget.FaceTargetWithVector3(currentTarget.GetComponent<Collider>().ClosestPoint(aimAssistCone.transform.position));
            }
        }



        // ---------- CAMERA STRAFE TILT ----------
        void UpdateCameraAimX(float mouseY)
        {

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        }

        void UpdateStrafeTiltZ()
        {
            /* STEPS:
             * - get velocity component along transform.right
             * - set target angle
             * - make a rotation that lerps to the target angle until it's hit
             */

            // grabbing component of cc speed along transform.right and converting it into a percentage
            float xProj;
            if (PlayerInputData.moveX == 0f)
            {
                xProj = 0;
            }

            // calculating projection on local positive x-axis
            else
            {
                float thetaDeg = Vector3.Angle(transform.right, cc.velocity);
                float thetaRad = Mathf.Deg2Rad * thetaDeg;
                xProj = -Mathf.Cos(thetaRad);
            }
            zRotation = Mathf.Lerp(zRotation, maxTilt * xProj, 5f * Time.deltaTime);
        }

        // ---------- FOV ----------
        public void UpdateFOV(float value /* unused */)
        {
            vcam.m_Lens.FieldOfView = settings.FOV;
        }

        void Update()
        {
            AimAssist();
        }

        void LateUpdate()
        {
            // gather inputs
            float mouseX = PlayerInputData.mouseXDelta * settings.MouseSensitivity * Time.deltaTime;
            float mouseY = PlayerInputData.mouseYDelta * settings.MouseSensitivity * Time.deltaTime;

            // update camera aim x rotation
            UpdateCameraAimX(mouseY);

            if (settings.UseCameraTilt == true)
            {
                // update strafe tilt z rotation
                UpdateStrafeTiltZ();
            }

            // apply all rotations
            transform.localRotation = Quaternion.Euler(xRotation, 0f, zRotation);

            // rotate player directly for y rotation
            Player.Rotate(Vector3.up * mouseX);
        }
    }
}
