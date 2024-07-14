using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using WSoft.Audio;

/*
 * A class that handles the triggering of a cutscene
 * Written by Allie Lavacek
 * 
 * User Guide:
        1. Place CutsceneTrigger script onto a gameobject with a 2d collider set to isTrigger
            i. This trigger will be responsible for telling the Cutscene Manager which cutscene to play
 */
namespace WSoft.Environment
{
    public class CutsceneTrigger : MonoBehaviour
    {
        [SerializeField] CutsceneManager cutsceneManager;
        [SerializeField] PlayableDirector CutScene;
        [SerializeField] AudioEvent cutsceneAudio;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                cutsceneManager.PlayCutscene(CutScene);
                cutsceneAudio.PlayAudio(gameObject);
                gameObject.SetActive(false);
            }
        }
    }
}