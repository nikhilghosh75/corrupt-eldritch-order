using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WSoft.Audio;

/*
 * A class that handles the basic operations of a switch
 * Written by Brandon Fox '24
 * 
 * User Guide:
        1. Place switch prefab in desired location in scene 
        2. Place GameObjects you want to appear/disappear (bridges, etc.) in scene at desired locations
        3. From the hierarchy, drag GameObjects into public controlledObjects array (If you want all the child of a gameobject to swap states, just drag the an empty parent gameobject in NOTE: THE PARENT WONT SWAP STATES JUST THE CHILDREN!)
            3a. This can be found by selecting the switch prefab and looking at the inspector
        4. Be sure to set active states of objects to desired states on start (active or inactive)
        5. Each time switch is hit, all controlled objects will have their active states reversed
 */

namespace WSoft.Environment
{
    public class SwitchController : MonoBehaviour
    {

        [Tooltip("Place objects that you want the switch to link to here")]
        public GameObject[] controlledObjects; //put into the order you want them to switch states of
        [Range(0, 2)]
        public float timeBetweenEachSwitch; //time between each switch option for jUiCe - allie

        public bool canSwitchMoreThanOnce;
        public bool controlGrandchildren = true;
        bool beenSwitched;

        private bool state = false;

        [SerializeField] AudioEvent switchSFX;

        [SerializeField] AudioEvent controlledObjectSFX;

        [SerializeField] Sprite switchedSprite;
        //to let encounter system or any script with a listener know that coroutine for switching finished
        //ex: incase want player movement or some enemies movement disabled until switched
        public UnityEvent FinishedSwitching;

        private void Start()
        {
            beenSwitched = false;
        }

        public void Flip()
        {
            if (canSwitchMoreThanOnce || !beenSwitched)
            {
                state = !state;
                //no coroutine needed if 0
                if (timeBetweenEachSwitch == 0)
                {
                    SwapStates();
                }
                else
                {
                    StartCoroutine(SwapStatesStaggered());
                }

                switchSFX.PlayAudio(gameObject);

                beenSwitched = true;
            }
        }

        private void SwapStates()
        {
            for (int i = 0; i < controlledObjects.Length; i++)
            {

                if (controlledObjects[i].transform.childCount > 0 && controlGrandchildren)
                {
                    //incase wanna group by parenting
                    foreach (Transform child in controlledObjects[i].transform)
                    {
                        if (child.gameObject.activeSelf && child.gameObject.GetComponent<Animator>())
                        {
                            child.gameObject.GetComponent<Animator>().Play("Despawn");
                        }
                        else
                        {
                            child.gameObject.SetActive(!child.gameObject.activeSelf);
                        }

                    }
                }
                else
                {
                    controlledObjects[i].gameObject.SetActive(!controlledObjects[i].gameObject.activeSelf);
                }

            }
            FinishedSwitching.Invoke();
            GetComponent<SpriteRenderer>().sprite = switchedSprite;
            controlledObjectSFX.PlayAudio(controlledObjects[0]);
        }

        IEnumerator SwapStatesStaggered()
        {
            for (int i = 0; i < controlledObjects.Length; i++)
            {
                if (controlledObjects[i].transform.childCount > 0)
                {
                    //incase wanna group by parenting
                    foreach (Transform child in controlledObjects[i].transform)
                    {
                        if (child.gameObject.activeSelf && child.gameObject.GetComponent<Animator>())
                        {
                            // if has animator try to set bool despawn to false, but if it isn't set, that means it doesnt exist
                            // and go with normal deactivate
                            child.gameObject.GetComponent<Animator>().SetBool("Despawn", true);

                            if (!child.gameObject.GetComponent<Animator>().GetBool("Despawn"))
                            {
                                child.gameObject.SetActive(!child.gameObject.activeSelf);
                            }
                        }
                        else
                        {
                            child.gameObject.SetActive(!child.gameObject.activeSelf);
                        }
                        yield return new WaitForSecondsRealtime(timeBetweenEachSwitch);
                    }
                }
                else
                {
                    controlledObjects[i].gameObject.SetActive(!controlledObjects[i].gameObject.activeSelf);
                    yield return new WaitForSecondsRealtime(timeBetweenEachSwitch);
                }

            }
            FinishedSwitching.Invoke();
            yield return null;
        }
    }
}