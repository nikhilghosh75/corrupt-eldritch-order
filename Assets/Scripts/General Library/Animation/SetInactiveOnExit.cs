using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Sets an object to be inactive as soon as it exists an animation state. Useful for controlling functionality through the animator alone.
 * Written by Jasmine Li '24
 */

namespace WSoft.Animation
{
    public class SetInactiveOnExit : StateMachineBehaviour
    {
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.gameObject.SetActive(false);
        }
    }
}