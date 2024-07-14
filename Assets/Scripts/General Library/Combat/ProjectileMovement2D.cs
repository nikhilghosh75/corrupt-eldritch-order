using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A class that moves a projectile at a given speed
 * Written by Nikhil Ghosh '24
 */

namespace WSoft.Combat
{
    public class ProjectileMovement2D : MonoBehaviour
    {
        public float speed;

        private Vector2 _direction;

        public Vector2 direction
        {
            get
            {
                return _direction;
            }
            set
            {
                // Update Velocity
                GetComponent<Rigidbody2D>().velocity = value.normalized * speed;

                // Set Rotation
                float angle = Mathf.Atan2(value.normalized.y, value.normalized.x) * Mathf.Rad2Deg;
                if (angle < 0)
                {
                    angle += 360;
                }
                transform.eulerAngles = new Vector3(0, 0, angle);

                // Store Value
                _direction = value;
            }
        }

    }
}