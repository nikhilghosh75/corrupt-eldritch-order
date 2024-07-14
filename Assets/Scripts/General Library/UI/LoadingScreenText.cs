using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace WSoft.UI
{
    public class LoadingScreenText : MonoBehaviour
    {
        public float time;
        TextMeshProUGUI text;

        // Start is called before the first frame update
        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
            StartCoroutine(DoLoadingScreenText());
        }

        IEnumerator DoLoadingScreenText()
        {
            int current = 0;
            while (true)
            {
                current = (current + 1) % 4;
                string str = "LOADING";
                for (int i = 0; i < current; i++)
                {
                    str += ".";
                }
                text.text = str;
                yield return new WaitForSeconds(time);
            }
        }
    }
}