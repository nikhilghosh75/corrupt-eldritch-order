using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A set of animation utility functions that are meant to be called by UnityEvents
 * Written by Matt Rader '19, Nikhil Ghosh '24
 */

namespace WSoft.Animation
{
	public class AnimatorUtils : MonoBehaviour
	{
		[SerializeField] private Animator _Animator;

		/// <summary>
		/// Set the Animator to update normally
		/// </summary>
		public void SetAnimatorModeNormal()
		{
			_Animator.updateMode = AnimatorUpdateMode.Normal;
		}

		/// <summary>
		/// Set the Animator to update during the physics loop
		/// </summary>
		public void SetAnimatorModeAnimatePhysics()
		{
			_Animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
		}

		/// <summary>
		/// Set the Animator to update independently of Time.timeScale
		/// </summary>
		public void SetAnimatorModeUnscaledTime()
		{
			_Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
		}

		/// <summary>
		/// Set the Animator to a given speed
		/// </summary>
		/// <param name="newSpeed">The speed to set the animator to</param>
		public void SetAnimatorSpeed(float newSpeed)
        {
			_Animator.speed = newSpeed;
        }
	}
}
