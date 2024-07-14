using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossName : MonoBehaviour
{
    [SerializeField] string title = "Beeg Kitty"; //name of boss

    [Tooltip ("0-explosive, 1-protective, 2-energized, 3-aura, 4-shockwave, 5-broodmother")]
    [SerializeField] string[] modeName1;//the suffix added for each boss mode when there is one mode
    [SerializeField] string[] modeName2;//the suffix added for each boss mode when there are two modes
    [SerializeField] string[] modeName3;//the prefix added for each boss mode when there are three modes
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeName(int[] modes)
    {
        string curTitle = title;
        if(modes.Length > 0)//if there is a mode, prepare to add suffixes
        {
            curTitle += ", the ";
        }

        if(modes.Length >= 3) // if three modes, there is a prefix
        {
            curTitle = modeName3[modes[2]] + " " + title;
        }
        if(modes.Length >= 2)//if two modes, there are two suffixes
        {
            curTitle += modeName2[modes[1]] + " ";
        }
        if(modes.Length >= 1)
        {
            curTitle += modeName1[modes[0]];
        }

        text.text = curTitle;
    }
}
