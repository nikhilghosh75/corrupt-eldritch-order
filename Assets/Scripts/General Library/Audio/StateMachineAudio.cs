/*
 * Used for audio inside a state machine (i.e. Animation Controller)
 * Written by Nikhil Ghosh '24
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Audio
{
    public enum StateMachineAudioMode
    {
        ENTER, // Play the audio only when entering the state
        PERIODIC, // Play the audio periodically
        ONFRAME,
        EXIT // Play the audio only when ending the state
    }

    public class StateMachineAudio : StateMachineBehaviour
    {
        [Tooltip("The Mode of the Audio")]
        public StateMachineAudioMode audioMode;

        public AudioEvent soundToPlay;

        [Tooltip("Only relevant if mode is set to PERIODIC")]
        public float periodTime;

        [Tooltip("Only relevant if mode is set to ONFRAME. This is in seconds")]
        public List<float> timesToPlay;

        float timeSinceLastPlayed = 0;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            timeSinceLastPlayed = 0;
            if (audioMode == StateMachineAudioMode.ENTER)
            {
                soundToPlay.PlayAudio(animator.gameObject);
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            timeSinceLastPlayed += Time.deltaTime;
            if (audioMode == StateMachineAudioMode.PERIODIC)
            {
                if (timeSinceLastPlayed >= periodTime)
                {
                    soundToPlay.PlayAudio(animator.gameObject);
                    timeSinceLastPlayed = 0;
                }
            }
            else if (audioMode == StateMachineAudioMode.ONFRAME)
            {
                float currentTimeInAnimation = GetCurrentTimeInAnimation(stateInfo);

                int nextIndexToPlay = -1;
                for (int i = 0; i < timesToPlay.Count; i++)
                {
                    if (currentTimeInAnimation < timesToPlay[i])
                    {
                        nextIndexToPlay = i;
                        break;
                    }
                }

                if (nextIndexToPlay != -1)
                {
                    float nextTimeToPlay = timesToPlay[nextIndexToPlay];
                    float lastTimeToPlay = (nextIndexToPlay == 0) ? 0.0f : timesToPlay[nextIndexToPlay - 1];

                    if (timeSinceLastPlayed >= nextTimeToPlay - lastTimeToPlay)
                    {
                        soundToPlay.PlayAudio(animator.gameObject);
                        timeSinceLastPlayed = 0;
                    }
                }
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (audioMode == StateMachineAudioMode.EXIT)
            {
                soundToPlay.PlayAudio(animator.gameObject);
            }
        }

        public static float GetCurrentTimeInAnimation(AnimatorStateInfo stateInfo)
        {
            float currentNormalizedTime = stateInfo.normalizedTime - (float)System.Math.Truncate(stateInfo.normalizedTime);
            float currentTimeInAnimation = currentNormalizedTime * stateInfo.length;
            return currentTimeInAnimation;
        }
    }
}