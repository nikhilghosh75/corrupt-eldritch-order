using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Audio;

public class TimeTrap : MonoBehaviour
{

    [Header("Time Trap Properties")]
    [SerializeField] bool isTimeTrap = true;
    [SerializeField] GameObject[] trap;
    [SerializeField] GameObject switchedPlatform;
    [SerializeField] float trapActivateTime = 7f;
    [SerializeField] float trapDeactivateTime = 7f;

    [SerializeField] AudioEvent timeTrapSwitch;

    // Start is called before the first frame update
    void Start()
    {
        switchedPlatform.SetActive(false);
        if (isTimeTrap)
        {
            StartCoroutine(Trap());
        }
    }

    IEnumerator Trap()
    {
        while (true)
        {
            if (trap != null)
            {
                for (int i = 0; i < trap.Length; i++)
                {
                    trap[i].SetActive(true);
                    switchedPlatform.SetActive(false);
                }
                GetComponent<Collider2D>().enabled = true;
                timeTrapSwitch.PlayAudio(gameObject);
            }
            yield return new WaitForSeconds(trapActivateTime);
            if (trap != null)
            {
                for (int i = 0; i < trap.Length; i++)
                {
                    trap[i].SetActive(false);
                    switchedPlatform.SetActive(true);
                }
                GetComponent<Collider2D>().enabled = false;
                timeTrapSwitch.PlayAudio(gameObject);
            }
            yield return new WaitForSeconds(trapDeactivateTime);
        }
    }
}
