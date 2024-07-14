/* Records for player relocation and level transition.
 * Attached to the Zone game object.
 * @Zhenyuan Zhang '?
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WSoft.Core
{
    public class TransitionScene : MonoBehaviour
    {
        [Tooltip("A gameObject that acts as an anchor point between the two levels. See Remarks for more info")]
        [SerializeField] private GameObject currentLevel;

        [Tooltip("The name of the anchor gameObject in the next level")]
        [SerializeField] private string nextLevelPath;

        [Tooltip("The name of the next scene")]
        [SerializeField] private string nextSceneName;

        [Tooltip("Objects inside region will be preserved to next scene")]
        [SerializeField] private Collider2D region;

        [Tooltip("Layer mask for preserving objects")]
        [SerializeField] private LayerMask layers;

        // The Player GameObject
        private GameObject player;

        [Tooltip("The offset of the player from the currentLevel GameObject")]
        private Vector3 playerOffset;

        private struct Preservation
        {
            [Tooltip("The gameObject to be preserved")]
            public GameObject gameObject;

            [Tooltip("The offset of the gameObject relative to the currentLevel GameObject")]
            public Vector3 offset;
        }
        private List<Preservation> preservations;

        private void Awake()
        {
            preservations = new List<Preservation>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                StartTransition();
            }
        }

        public void StartTransition()
        {
            StartCoroutine(DoTransition());
        }

        IEnumerator DoTransition()
        {
            CollectObjects();

            string currentSceneName = SceneManager.GetActiveScene().name;

            AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
            yield return new WaitUntil(() => { return operation.isDone; });

            AfterLoading();

            SceneManager.UnloadSceneAsync(currentSceneName);
        }

        /// <summary>
        /// Finds all GameObjects that overlap the collider in the specified layermask and makes a preservation object
        /// </summary>
        private void CollectObjects()
        {
            var filter = new ContactFilter2D();
            filter.SetLayerMask(layers);

            var colliders = new List<Collider2D>();
            Physics2D.OverlapCollider(region, filter, colliders);

            var objects = new List<GameObject>();
            foreach (var collider in colliders)
            {
                objects.Add(collider.gameObject);
            }

            foreach (var obj in objects)
            {
                var preservation = new Preservation
                {
                    gameObject = obj,
                    offset = obj.transform.position - currentLevel.transform.position
                };
                preservation.offset.z = 0;
                preservations.Add(preservation);
            }
        }

        /// <summary>
        /// On loading the new scene, reload all the GameObjects at a specified offset from the corresponding GameObject in the scene.
        /// </summary>
        private void AfterLoading()
        {
            var nextScene = SceneManager.GetSceneByName(nextSceneName);
            var nextLevel = GameObject.Find(nextLevelPath);

            foreach (var preservation in preservations)
            {
                var obj = preservation.gameObject;
                var offset = preservation.offset;

                if (obj.transform.parent != null)
                    obj.transform.SetParent(null);
                SceneManager.MoveGameObjectToScene(obj, nextScene);

                obj.transform.position = nextLevel.transform.position + offset;
            }

            playerOffset = player.transform.position - currentLevel.transform.position;
            player.SetActive(false);
            Destroy(player);

            player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = nextLevel.transform.position + playerOffset;

            player.SetActive(false);
            player.SetActive(true);
        }
    }
}

