using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatDisplayBar : MonoBehaviour
{
    public Sprite filledCell, emptyCell;

    private List<Image> statCells;
    private int numCells;

    void Start()
    {
        GameObject displayBar = transform.GetChild(1).gameObject;
        numCells = displayBar.transform.childCount;
        statCells = new List<Image>();
        for (int i = 0; i < numCells; i++)
        {
            statCells.Add(displayBar.transform.GetChild(i).GetComponent<Image>());
        }

    }

    public void UpdateStatBar(float percentage)
    {
        if (percentage > 0) {
            float numFilledCells = Mathf.Max(1, Mathf.RoundToInt(percentage * numCells));

            for (int i = 0; i < numCells; i++)
            {
                if (i < numFilledCells)
                {
                    statCells[i].sprite = filledCell;
                }
                else
                {
                    statCells[i].sprite = emptyCell;
                }
            }
        }

    }
}
