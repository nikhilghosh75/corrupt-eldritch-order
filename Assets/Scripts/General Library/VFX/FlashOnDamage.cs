using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Combat;

/*
 * Given a set of sprites, makes them flash a certain color upon taking damage
 * Written by Bob Zhang '23
 */

namespace WSoft.VFX
{
    public class FlashOnDamage : MonoBehaviour
    {
        // A list of sprites relevant to visually representing this enemy
        public SpriteRenderer[] sprites;

        // Color to flash when taking damage
        public Color flashColor = Color.red;

        public float flashDuration = 0.5f;

        HealthSystem health;

        // Start is called before the first frame update
        void Start()
        {
            health = GetComponent<HealthSystem>();
            health.events.OnDamage.AddListener(FlashDamageColor);

        }

        // Update is called once per frame
        void Update()
        {

        }

        //turn on flash a certain color (when hit/damaged)
        public void FlashDamageColor()
        {
            StartCoroutine(TurnSpritesColor(flashDuration));
        }

        //coroutine for turning enemy red for specified amount of time
        IEnumerator TurnSpritesColor(float time)
        {
            //turn all relevant sprites a certain color
            foreach (SpriteRenderer sprite in sprites)
                sprite.color = flashColor;

            //wait for specified time
            yield return new WaitForSeconds(time);

            //turn all relevant sprites back to white/default color
            foreach (SpriteRenderer sprite in sprites)
                sprite.color = Color.white;
        }
    }
}
