using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Destroys an object as soon as it exists an animation state. Useful for controlling functionality through the animator alone.
 * Written by Jasmine Li '24
 */

namespace WSoft.Animation
{
    public class DestroyOnExit : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(animator.gameObject, stateInfo.length);
        }
    }
}