using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool Roar = false;
    public bool Moving = false;
    public bool Aim = false;
    public float AimVal = 0;
    public bool ExitAim = false;
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        if (Roar) 
        {
            Roar = false;

            anim.Play("BossRoar");
        }
        if (Aim)
        {
            Aim = false;
            anim.Play("BeginAim");
            anim.SetBool("Aiming", true);
        }
        anim.SetFloat("Aim", AimVal);
        anim.SetBool("Moving", Moving);
        if (ExitAim) 
        {
            ExitAim = false;
            anim.SetBool("Aiming", false);
        }
    }

    public float RadiansToBossAimAngle(float radians)
    {
        float clampedAngle = radians % (2f * Mathf.PI);

        if (clampedAngle < Mathf.PI / 2f)
        {
            return clampedAngle / (Mathf.PI / 2f);
        }
        else if (clampedAngle < (3f * Mathf.PI / 2f))
        {
            return (clampedAngle - Mathf.PI) / (Mathf.PI / 2f);
        }
        else
        {
            return (clampedAngle - (3f * Mathf.PI / 2f)) / (Mathf.PI / 2f) * -1f;
        }
    }
}
