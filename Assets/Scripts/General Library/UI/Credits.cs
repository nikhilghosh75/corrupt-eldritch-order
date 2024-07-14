/*
 * A framework for a credits system
 * Written by Natasha Badami '20, George Castle '22, Nikhil Ghosh '24, Henry Lin '23
 * 
 * This code may have aspects/assumptions that were specific to its original project. 
 * For instance, you should modify the dictionary with the names of the team members for your project
 * I would recommend using it as a reference (when implementing a new script), rather than purely copying it and pasting it
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace WSoft.UI
{
    public class Credits : MonoBehaviour
    {
        public GameObject startPrefab;
        public GameObject titlePrefab;
        public GameObject namePrefab;
        public GameObject endPrefab;

        public float startPosition;
        public float spacingBetweenSections;
        public float spacingBetweenNames;
        public float scrollSpeed;

        public bool scrollOnStart = false;

        public UnityEvent OnComplete;

        public bool scrolling = false;

        private float goalPosition = 0f;

        private float finishTime;

        private RectTransform rectTransform;

        [System.Serializable]
        public struct Segment
        {
            public string teamName;
            public List<string> members;
        }

        public List<Segment> segments;

        // Start is called before the first frame update
        void Start()
        {
            goalPosition = startPosition;
            createNode(startPrefab);
            foreach (Segment segment in segments)
            {
                createPair(segment.teamName, segment.members);
            }

            createNode(endPrefab);

            rectTransform = GetComponent<RectTransform>();

            scrolling = scrollOnStart;
        }

        void Update()
        {
            if (scrolling)
            {
                if (rectTransform.anchoredPosition.y < goalPosition)
                {
                    rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
                }
                else if (finishTime == 0)
                {
                    finishTime = Time.unscaledTime;
                }

                if (finishTime > 0 && Time.unscaledTime - finishTime > 2f)
                {
                    OnComplete.Invoke();
                }
            }
        }

        public void StartScroll()
        {
            scrolling = true;
        }

        public void StopScroll()
        {
            scrolling = false;
            finishTime = 0;
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
            rectTransform.anchoredPosition = new Vector2(0, 0);
        }

        void createPair(string title, List<string> names)
        {
            createNode(titlePrefab).GetComponent<TMPro.TextMeshProUGUI>().text = title;
            foreach (string name in names)
            {
                createNode(namePrefab).GetComponent<TMPro.TextMeshProUGUI>().text = name;
            }
            goalPosition += spacingBetweenSections;
        }

        GameObject createNode(GameObject prefab)
        {
            GameObject newObj = Instantiate(prefab);
            newObj.transform.SetParent(transform);

            RectTransform rect = newObj.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, -goalPosition);

            goalPosition += rect.GetHeight() + spacingBetweenNames;

            return newObj;
        }
    }
}