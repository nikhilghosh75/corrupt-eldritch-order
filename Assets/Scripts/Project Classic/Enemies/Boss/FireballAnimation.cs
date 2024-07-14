using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAnimation : MonoBehaviour
{
    // List of sprites to cycle through
    [SerializeField] List<Sprite> sprites;

    // Time between each sprite
    [SerializeField] float cycleTime = 0.2f;

    float lastSpriteChangeTime = 0f;

    SpriteRenderer sr;

    int spriteIndex = 0;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSpriteChangeTime > cycleTime)
        {
            spriteIndex = (spriteIndex + 1) % sprites.Count;
            sr.sprite = sprites[spriteIndex];
            lastSpriteChangeTime = Time.time;
        }
    }
}
