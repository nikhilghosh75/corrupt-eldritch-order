using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverKilledBy : MonoBehaviour
{
    [Serializable]
    public class KilledBySprite
    {
        public Sprite sprite;
        public string name;
    }

    public List<KilledBySprite> namesToSprites;
    public Image image;
    public TMP_Text text;

    Dictionary<string, Sprite> sprites;

    public void ResetKilledBy()
    {
        image.sprite = null;
    }

    public IEnumerator ShowKilledBy()
    {
        PopulateDictionary();

        string lastKilledBy = ProgressionManager.instance.lastDamagedBy;
        image.sprite = sprites[lastKilledBy];
        text.text = lastKilledBy;

        yield return new WaitForSecondsRealtime(0.5f);
    }

    void PopulateDictionary()
    {
        sprites = new Dictionary<string, Sprite>();
        foreach(KilledBySprite sprite in namesToSprites)
        {
            sprites.Add(sprite.name, sprite.sprite);
        }
    }
}
