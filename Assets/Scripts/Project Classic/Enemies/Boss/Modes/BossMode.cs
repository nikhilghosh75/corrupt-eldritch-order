using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossMode : MonoBehaviour
{
    /*
    0 explosive
    1 protective
    2 energized
    3 aura
    4 shockwave
    5 broodmother
     */
    public virtual int ModeID { get { return -1; } }

    // Called before beginning of Boss's behavior cycle
    public abstract void OnBehaviorStart();

    // Called after an attack
    public virtual void OnPostAttack()
    {
        return;
    }
}
