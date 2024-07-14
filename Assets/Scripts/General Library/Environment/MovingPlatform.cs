using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A script that controls a moving platform
 * Written by Michael Stowe '21, Steven Tam '21, Nikhil Ghosh '24
 */

namespace WSoft.Environment
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] Transform pointA;
        [SerializeField] Transform pointB;
        [SerializeField] float movingPlatformTime;
        public bool moveOnAwake;
        float lerpToAPercent;
        float lerpToBPercent;

        public bool moving { get; set; }
        bool movingToB;

        void Start()
        {
            moving = moveOnAwake;
        }

        // Update is called once per frame
        void Update()
        {
            if (moving)
            {
                if (movingToB)
                {
                    transform.position = Vector3.Lerp(pointA.position, pointB.position, lerpToBPercent);
                    lerpToBPercent += Time.deltaTime / movingPlatformTime;

                    if (lerpToBPercent > 1)
                    {
                        moving = false;
                    }
                }
                else
                {
                    transform.position = Vector3.Lerp(pointB.position, pointA.position, lerpToAPercent);
                    lerpToAPercent += Time.deltaTime / movingPlatformTime;

                    if (lerpToAPercent > 1)
                    {
                        moving = false;
                    }
                }
            }
        }

        public void MoveToB()
        {
            moving = true;
            movingToB = true;
        }

        public void MoveToA()
        {
            moving = true;
            movingToB = false;
        }
    }
}