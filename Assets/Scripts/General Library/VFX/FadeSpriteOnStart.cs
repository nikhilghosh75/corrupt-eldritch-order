using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Used to fade out a sprite at start. Used primarily for sprite-based effects
 * Written by Henry Lin '23
 */

namespace WSoft.VFX
{
    public class FadeSpriteOnStart : MonoBehaviour
    {
        public float fadeTime = 3.0f;
        public bool destroyOnFade = true;

        SpriteRenderer spriteRenderer;

        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            StartCoroutine(Fade());
        }

        IEnumerator Fade()
        {
            float startAlpha = spriteRenderer.color.a;
            float endAlpha = 0.0f;

            float startTime = Time.time;
            float progress = (Time.time - startTime) / fadeTime;
            while (progress < 1.0f)
            {
                spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Lerp(startAlpha, endAlpha, progress));
                progress = (Time.time - startTime) / fadeTime;
                yield return null;
            }
            spriteRenderer.color = Color.clear;

            if (destroyOnFade)
                Destroy(gameObject);
        }
    }
}