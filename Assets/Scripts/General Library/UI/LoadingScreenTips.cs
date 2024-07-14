using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace WSoft.UI
{
    public class LoadingScreenTips : MonoBehaviour
    {
        public List<string> tips;
        public float time;
        TextMeshProUGUI text;

        // Start is called before the first frame update
        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
            StartCoroutine(DoTips());
        }

        IEnumerator DoTips()
        {
            int randIndex = 0;
            while (true)
            {
                int lastRandIndex = randIndex;
                randIndex = Random.Range(0, tips.Count);

                if (lastRandIndex == randIndex)
                    randIndex = (lastRandIndex + 1) % tips.Count;

                text.text = tips[randIndex];

                yield return new WaitForSeconds(time);
            }
        }
    }
}