using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public GameObject exitReplacement;
    public LevelDirection direction;
    public List<LevelExit> incompatibleExits;

    [HideInInspector]
    public bool removed = false;
    Color originalSpriteColor;

    public void Remove()
    {
        removed = true;

        exitReplacement.layer = LayerMask.NameToLayer("Door");
        originalSpriteColor = exitReplacement.GetComponent<SpriteRenderer>().color;
        exitReplacement.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
    }

    public void Add()
    {
        removed = false;
        exitReplacement.layer = LayerMask.NameToLayer("Ground");
        exitReplacement.GetComponent<SpriteRenderer>().color = originalSpriteColor;
    }
}
