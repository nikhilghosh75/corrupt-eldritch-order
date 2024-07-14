using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

/*
 * A class that handles the cutscenes
 * Written by Allie Lavacek
 * 
 * User Guide:
        1. Place Cutscene Manager prefab into the scene if one has not already been placed
            i. Any new cutscenes can be placed as a child of this prefab's Cutscenes child, look more into Timelines
                to understand how timelines work
            ii. Make sure player's animation culling is set to Cull Update transform, along with any other gameobject you plan to animate and then use
                with the timeline.  Note: cinemachines automatically follow what they are set to follow, so animations will not have to be added to them
                to achieve an effect as scene with the sample cutscene.  Rather, create a new vcam and change its to follow
        2. If you would like a cutscene right at the beginning of the scene, place said cutscene into the OpeningSceneCutscene, and if you only want
                cutscenes that are triggered, create a cutscenetrigger instance accordingly
        Note: It is best to implement audio inside of the according cutscene Timeline!  That way lining up when the audio finishes and when certain events happen
            in the cutscene won't look rushed or awkward
 */
namespace WSoft.Environment
{
    public class CutsceneManager : MonoBehaviour
    {
        [Tooltip("Optional - only to be played once per level")]
        [SerializeField] PlayableDirector OpeningSceneCutscene;

        Coroutine coroutine;
        PlayableDirector currentCutscene;

        [SerializeField] PlayerInput playerInput;

        // reference to Narrative gameObject
        [SerializeField] GameObject _NarrativeRef;

        // name of played cutscene
        public static string playedCutscene = null;

        // Start is called before the first frame update
        void Start()
        {
            if (OpeningSceneCutscene && (playedCutscene == null || playedCutscene != OpeningSceneCutscene.name))
            {
                playedCutscene = OpeningSceneCutscene.name;
                _NarrativeRef.SetActive(true);
                PlayCutscene(OpeningSceneCutscene);
            }
            else if (OpeningSceneCutscene == null)
            {
                _NarrativeRef.SetActive(false);
            }
            // attempt at preventing Narrative gameobject from appearing after death
            else if (playedCutscene == OpeningSceneCutscene.name)
            {
                _NarrativeRef.SetActive(false);
            }
        }

        public void PlayCutscene(PlayableDirector CutScene)
        {
            if (CutScene == OpeningSceneCutscene)
            {
                _NarrativeRef.SetActive(true);

                if (playerInput != null)
                    playerInput.actions.Disable();
                coroutine = StartCoroutine(WaitForCutsceneToEnd(CutScene));
            }
            else
            {
                CutScene.Play();
                if (_NarrativeRef.activeSelf)
                {
                    coroutine = StartCoroutine(WaitForCutsceneToEnd(CutScene));
                }
            }

        }

        public void StopCutscene()
        {
            StopCoroutine(coroutine);
            currentCutscene.Stop();
            if (playerInput != null)
                playerInput.actions.Enable();
            Time.timeScale = 1;
        }

        private IEnumerator WaitForCutsceneToEnd(PlayableDirector CutScene)
        {
            //pause normal game player while cutscene is active
            float timeScale = Time.timeScale;
            Time.timeScale = 0;

            currentCutscene = CutScene;
            CutScene.Play();

            yield return new WaitForSecondsRealtime((float)CutScene.duration);

            Time.timeScale = timeScale;
            if (playerInput != null)
                playerInput.actions.Enable();
            currentCutscene = null;
            yield return null;

            // hide Narrative gameobject after ending
            _NarrativeRef.SetActive(false);
        }
    }
}