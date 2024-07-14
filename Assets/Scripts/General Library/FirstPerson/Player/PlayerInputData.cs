/*
 * A script used in Bloom: A Tomb of Power. Used with the Player movement script to store data regarding the players input (movement, camera, Bloom specific: weapon scroll sensitivity).
 * REQUIRES: 1) Used with the PlayerMovement script in the FirstPerson folder.
 * Written by Natasha Badami '20, Matt Rader '19
 * 
 * This code may have aspects/assumptions that were specific to its original project. 
 * I would recommend using it as a reference (when implementing a new script), rather than purely copying it and pasting it
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WSoft.FirstPerson
{
    public class PlayerInputData : MonoBehaviour
    {
        [Range(0.05f, 1f)]
        public float scrollSensitivity;

        public static float moveX { get; private set; }
        public static float moveZ { get; private set; }
        public static float mouseXDelta { get; private set; }
        public static float mouseYDelta { get; private set; }
        public static int weaponIndex { get; private set; }
        public static bool weaponIndexSelectedThisFrame { get; private set; }
        public static bool weaponIndexIncrementedThisFrame { get; private set; }
        public static bool weaponIndexDecrementedThisFrame { get; private set; }
        public static bool isTryingCast { get; private set; }
        public static bool attemptedCastThisFrame { get; private set; }
        public static bool isTryingJump { get; private set; }

        // tracks how much the player has scrolled
        // resets to 0 when weapon switch occurs
        static float scrollAmount;

        public void OnMove(InputValue value)
        {
            moveX = value.Get<Vector2>().x;
            moveZ = value.Get<Vector2>().y;
        }

        public void OnLook(InputValue value)
        {
            mouseXDelta = value.Get<Vector2>().x;
            mouseYDelta = value.Get<Vector2>().y;
        }

        // Invoked on press and release
        public void OnJump(InputValue value)
        {
            isTryingJump = value.isPressed;
        }

        // Invoked on press and release
        public void OnFire(InputValue value)
        {
            isTryingCast = value.isPressed;
            attemptedCastThisFrame = value.isPressed;
        }

        // Weapon switching
        public void OnWeapon1(InputValue value) { SetWeaponIndex(0); }
        public void OnWeapon2(InputValue value) { SetWeaponIndex(1); }
        public void OnWeapon3(InputValue value) { SetWeaponIndex(2); }
        public void OnWeapon4(InputValue value) { SetWeaponIndex(3); }
        public void OnWeapon5(InputValue value) { SetWeaponIndex(4); }

        private void SetWeaponIndex(int index)
        {
            weaponIndex = index;
            weaponIndexSelectedThisFrame = true;
        }

        void Awake()
        {
            //Mouse.current.scroll.y.clamp = UnityEngine.InputSystem.Controls.AxisControl.Clamp.BeforeNormalize;
            //Mouse.current.scroll.y.clampMax = 1 / scrollSensitivity;
            //Mouse.current.scroll.y.clampMin = -1 / scrollSensitivity;
        }

        void Update()
        {
            scrollAmount += Time.deltaTime * Mouse.current.scroll.y.ReadValue();
            if (scrollAmount >= 1 / scrollSensitivity)
            {
                scrollAmount = 0f;
                weaponIndexIncrementedThisFrame = true;
            }
            else if (scrollAmount <= -1 / scrollSensitivity)
            {
                scrollAmount = 0f;
                weaponIndexDecrementedThisFrame = true;
            }
        }

        // LateUpdate is called once per frame after all Update calls
        void LateUpdate()
        {
            weaponIndexSelectedThisFrame = false;
            weaponIndexIncrementedThisFrame = false;
            weaponIndexDecrementedThisFrame = false;
            attemptedCastThisFrame = false;
        }
    }
}
