/*
 * Creates a parallax effect where the background and foreground move at different speeds.
 * @ Zhang Zhenyuan '?, David Sohn '20, Max Perraut '20
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Camera
{
    public class Parallax2D : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private float startPos;
        private float length;
        private float startPos2;
        private float height;
        [SerializeField] GameObject worldCamera;

        [SerializeField] bool keepInFrame = true;
        [SerializeField] bool vertical;
        [SerializeField] bool lockedToCamera;
        [SerializeField] float horizontalParallaxEffect;
        [SerializeField] float verticalParallaxEffect;

        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            startPos = transform.position.x;
            startPos2 = transform.position.y;
            if(spriteRenderer != null)
            {
                length = spriteRenderer.bounds.size.x;
                height = spriteRenderer.bounds.size.y;
            }
        }

        // Update is called once per frame
        void Update()
        {
            float temp = (worldCamera.transform.position.x * (1 - horizontalParallaxEffect));
            float dist = (worldCamera.transform.position.x * horizontalParallaxEffect);

            transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

            if (vertical)
            {
                float temp2 = (worldCamera.transform.position.y * (1 - verticalParallaxEffect));
                float dist2 = (worldCamera.transform.position.y * verticalParallaxEffect);
                transform.position = new Vector3(transform.position.x, startPos2 + dist2, transform.position.z);
                if (temp2 > startPos2 + height)
                {
                    startPos2 += height;
                }
                else if (temp2 < startPos2 - height)
                {
                    startPos2 -= height;
                }
            }
            else if (lockedToCamera)
            {
                transform.position = new Vector3(transform.position.x, worldCamera.transform.position.y, transform.position.z);
            }

            if (keepInFrame)
            {
                if (temp > startPos + length) startPos += length;
                else if (temp < startPos - length) startPos -= length;
            }
        }
    }
}