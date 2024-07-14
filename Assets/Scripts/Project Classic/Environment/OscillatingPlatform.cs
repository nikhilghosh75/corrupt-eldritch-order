using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A script that controls a moving/oscillating platform
 * Written by Michael Stowe '21, Steven Tam '21, Nikhil Ghosh '24, Eli Fox '24
 */

public class OscillatingPlatform : MonoBehaviour
{
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float movingPlatformTime;
    [SerializeField] float dwellTime;
    [SerializeField][Tooltip("Whether the platform moves back and forth automatically")] bool oscillating;
    public bool moveOnAwake;
    float lerpPercent;

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
                transform.position = Vector3.Lerp(pointA.position, pointB.position, lerpPercent);
                lerpPercent += Time.deltaTime / movingPlatformTime;

                if (lerpPercent >= 1)
                {
                    moving = false;
                    // Move to A after waiting for dwellTime seconds
                    if (oscillating)
                    {
                        Invoke(nameof(MoveToA), dwellTime);
                    }
                }
            }
            else
            {
                transform.position = Vector3.Lerp(pointA.position, pointB.position, lerpPercent);
                lerpPercent -= Time.deltaTime / movingPlatformTime;

                if (lerpPercent <= 0)
                {
                    moving = false;
                    // Move to B after waiting for dwellTime seconds
                    if (oscillating)
                    {
                        Invoke(nameof(MoveToB), dwellTime);
                    }
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