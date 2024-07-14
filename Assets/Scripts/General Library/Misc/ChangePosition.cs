using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Changes the position of a game object to a preset position
 * Useful for UnityEvents
 * Written by Natasha Badami '20, Alex Czarnecki '22
 */

namespace WSoft
{
    public class ChangePosition : MonoBehaviour
    {
        public Vector3 pos;

        public void SetPosToMyPos()
        {
            transform.position = pos;
        }
    }
}