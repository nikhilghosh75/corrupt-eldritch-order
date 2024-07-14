using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Audio;

public class FootstepsAudio : MonoBehaviour
{
    [Header("Player Movement Audio")]
    public AudioEvent playFootsteps;
    public AudioEvent stopFootsteps;
    public bool isWalking;

    PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        if (controller.horizontal != 0 && controller.IsGrounded() && !isWalking)
        {
            isWalking = true;
            playFootsteps.PlayAudio(gameObject);
        }

        if (controller.horizontal == 0 && controller.IsGrounded() && isWalking)
        {
            isWalking = false;
            stopFootsteps.PlayAudio(gameObject);
        }

        if (!controller.IsGrounded() && isWalking)
        {
            isWalking = false;
            stopFootsteps.PlayAudio(gameObject);
        }
    }
}
