using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType 
{ 
    Default,
    Ninja,
    Wizard,
    Tank

};

public class AnimationCharacterController : MonoBehaviour
{
    public GameObject ninjaObject;
    public GameObject wizardObject;
    public GameObject defaultObject;
    public GameObject tankObject;
    public Animator anim;

    public CharacterType characterType;
    public PlayerController player;

    public void Update()
    {
        anim.SetFloat("Aim", player.aimDirection.y);

        if (Mathf.Abs(player.rb.velocity.x) > 0.01)
            SetMovementDirection((player.aimDirection.x * player.rb.velocity.x > 1) ? 0 : 2); // Set to 0 if aim and movement directions are the same, 2 otherwise
        else
            SetMovementDirection(1);
    }
    public void TriggerDodge() 
    {
        Debug.Log("Player_Dodged");
        anim.SetTrigger("DodgeRoll");
    }

    public void TriggerJump() 
    {
        Debug.Log("Player_Jump");
        anim.SetTrigger("Jump");
    }

    public void TriggerLandGround() 
    {
        Debug.Log("Player_G_Land");
        anim.SetTrigger("Land_G");
    }

    public void TriggerLandWall()
    {
        Debug.Log("Player_W_Land");
        anim.SetTrigger("Land_W");
    }

    public void SetValue(bool Grounded, bool Moving, bool OnWall) 
    {
        anim.SetBool("OnGround", Grounded);
        anim.SetBool("Moving", Moving);
        anim.SetBool("OnWall", OnWall);
    }

    public void SetMovementDirection(float dir) 
    {
        // this is a bit weird but 0 is forward, 1 is idle, 2 is backwards
        anim.SetFloat("MoveDirection", dir);
        
    }

    public void HandleAim(float aim) 
    {
        // more weird stuff but 0 is up straight ahead is .5 and 1 is down
        anim.SetFloat("Aim", aim);
    }

    public void ActivateCharacterGraphic(CharacterType characterSelection)
    {
        defaultObject.SetActive(false);
        ninjaObject.SetActive(false);
        tankObject.SetActive(false);
        wizardObject.SetActive(false);
        switch (characterSelection) 
        {
            case CharacterType.Default:
                defaultObject.SetActive(true);
                break;
            case CharacterType.Ninja:
                ninjaObject.SetActive(true);
                break;
            case CharacterType.Tank:
                tankObject.SetActive(true);
                break;
            case CharacterType.Wizard:
                wizardObject.SetActive(true);
                break;
        }
    }
}
