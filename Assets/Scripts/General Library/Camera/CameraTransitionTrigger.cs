using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

/*
 * A trigger that enables a camera transition. Requires cinemachine to work
 * Written by Allie Lavacek '24
 */
namespace WSoft.Camera
{
    public class CameraTransitionTrigger : MonoBehaviour
    {
        [SerializeField] bool canReEnterCurrentArea;

        [SerializeField] string playerTag;

        //black screen or image covering the screen while scene transition takes place
        [Tooltip("Optional")]
        [SerializeField] Image transitionScreen;

        public bool shouldTeleport;

        public Vector3 teleportPlayerTo;

        //make the cinamachine you want to transition to has its confiner 2d component set (if you want bounds on that section of the map's camera)
        //if you do have a bounding shape set, make sure the cinamachine is inside those bounds.
        [SerializeField] CinemachineVirtualCamera cinamachine;

        [Tooltip("If transition screen is left to none this will be disregarded")]
        [SerializeField] float transitionTime;
        private GameObject player;

        private void Start()
        {
            //make sure screen starts transparent
            if (transitionScreen)
            {
                transitionScreen.color = new Color(transitionScreen.color.r, transitionScreen.color.g, transitionScreen.color.b, 0f);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(playerTag))
            {
                player = other.gameObject;
                StartCoroutine(FadeAndTeleport());
            }
        }

        IEnumerator FadeAndTeleport()
        {
            float fadeOutAmount = 0.0f;
            Color screenColor = Color.black;
            if (transitionScreen)
            {
                screenColor = transitionScreen.GetComponent<Image>().color;
                //should already have a set to 0 but just in case
                transitionScreen.color = new Color(screenColor.r, screenColor.g, screenColor.b, 0f);

                //fade in transition screen
                while (transitionScreen.GetComponent<Image>().color.a < 1)
                {
                    fadeOutAmount = transitionScreen.color.a + (5.0f * Time.deltaTime * 1 / (transitionTime / 2));
                    transitionScreen.color = new Color(screenColor.r, screenColor.g, screenColor.b, fadeOutAmount);

                    yield return new WaitForSeconds(0.025f);
                }
            }


            //teleport player
            if (shouldTeleport)
                player.transform.position = teleportPlayerTo;

            //set new active camera and switch old one for when they reenter
            CinemachineVirtualCamera temp;
            temp = UnityEngine.Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
            UnityEngine.Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.SetActive(false);
            cinamachine.VirtualCameraGameObject.SetActive(true);
            cinamachine = temp;

            //make into collider instead of trigger if cannot reenter area
            if (!canReEnterCurrentArea)
            {
                gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            }

            if (transitionScreen)
            {
                yield return new WaitForSeconds(transitionTime / 2);

                //done moving player and switching camera, fade out screen
                while (transitionScreen.color.a > 0)
                {
                    fadeOutAmount = transitionScreen.color.a - (5.0f * Time.deltaTime * 1 / (transitionTime / 2));
                    transitionScreen.color = new Color(screenColor.r, screenColor.g, screenColor.b, fadeOutAmount);

                    yield return new WaitForSeconds(0.025f);
                }
            }
            yield break;
        }
    }
}