using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAction : MonoBehaviour
{
    // Used to get how long this enemy action will last for
    // Used for timing enemy cycles
    public abstract float GetActionTime();

    // Called by an enemy's behavior loop
    // Performs the designated action for the script
    public abstract void Act();

    // Optional function that can be use in Coroutines to check if an action is complete
    public virtual bool ActionFinished()
    {
        return true;
    }
    
    // Called when an enemy needs to stop the current action (such as being frozen)
    public abstract void Interrupt();
}
