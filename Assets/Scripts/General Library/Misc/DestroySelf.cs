using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * A class that destroys a game object on a function call
 * Useful for Unity Events
 * Written by Natasha Badami '20, Zheng Zhenyuan '21, Jackson Valis '22
 */

namespace WSoft
{
    public class DestroySelf : MonoBehaviour
    {
        public UnityEvent OnDestory;

        public void SelfDestruct()
        {
            OnDestory?.Invoke();
            Destroy(gameObject);
        }

        public void SelfDestructAfter(float timer)
        {
            if (gameObject != null)
            {
                Destroy(gameObject, timer);
            }
        }
    }
}