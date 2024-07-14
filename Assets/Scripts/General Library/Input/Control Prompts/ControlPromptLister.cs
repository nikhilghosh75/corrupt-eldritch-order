using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A UI Element that lists a Control Prompt Database
 * Written by Zhenyuan Zhang '21?
 */

namespace WSoft.Input.ControlPrompt
{
    public class ControlPromptLister : MonoBehaviour
    {
        public ControlPromptDatabase database;
        [SerializeField] GameObject prefab;
        [SerializeField] bool generateOnStart = true;

        public Dictionary<string, GameObject> children { get; private set; } = new Dictionary<string, GameObject>();

        void Start()
        {
            if (generateOnStart) Generate();
        }

        /// <summery>
        /// Instantiate all entries from the database.
        /// </summery>
        public void Generate()
        {
            if (database == null) return;
            foreach (var controlPrompt in database)
            {
                GameObject obj = Instantiate(prefab, transform);
                var view = obj.GetComponent<ControlPromptView>();
                view.controlPrompt = controlPrompt;

                children.Add(controlPrompt.actionName, obj);
            }
        }

        public GameObject Find(string action) => children[action];

        public void SetActiveAll(bool active)
        {
            foreach (var obj in children.Values)
                obj.SetActive(active);
        }
    }
}