/*
 * Makes a platform collapse after the player steps on it.
 * Destroys object after certain time falling or time
 * on ground.
 * @Alex Pohlman
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WSoft.Environment
{
    public class CollapsingPlatform : MonoBehaviour
    {
        [Tooltip("Time that platform shakes for until it falls")]
        [SerializeField] float timeTillCollapse = 0f;

        [Tooltip("Time that platform lays on the ground for until it is destroyed")]
        [SerializeField] float timeTillDestroy = 0f;

        [Tooltip("Time that platform can fall for until it is destroyed")]
        [SerializeField] float timeFalling = 0f;

        [Tooltip("Distance that platform shakes by")]
        [SerializeField] float shakeDistance = 0f;

        [Tooltip("Speed at which platform shakes")]
        [SerializeField] float shakeSpeed = 0f;
        Rigidbody rb;
        bool isShaking = false;
        bool isFalling = false;

        float initialX;
        float initialZ;
        float initialY;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();

            // Gets platform's initial position
            initialX = transform.position.x;
            initialY = transform.position.y;
            initialZ = transform.position.z;
        }

        // Update is called once per frame
        void Update()
        {
            // Shakes the platform before it begins to fall
            if (isShaking)
            {
                float newX = initialX + Random.Range(-shakeDistance, shakeDistance);
                float newY = initialY + Random.Range(-shakeDistance, shakeDistance);
                float newZ = initialZ + Random.Range(-shakeDistance, shakeDistance);
                Vector3 newPosition = new Vector3(newX, newY, newZ);
                transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * shakeSpeed);
            }
        }

        /// <summary>
        /// Starts shaking and falling when player steps on platform, 
        /// destroys it self after it contacts the ground
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                isShaking = true;
                StartCoroutine(Fall());
            }

            else if (!other.isTrigger)
            {
                isFalling = false;
                StartCoroutine(Destroy());
            }

        }

        /// <summary>
        /// Spends time shaking, then activates gravity and starts falling
        /// </summary>
        private IEnumerator Fall()
        {
            yield return new WaitForSeconds(timeTillCollapse);
            isShaking = false;
            rb.useGravity = true;
            isFalling = true;
            StartCoroutine(DestroyWhenFalling());
        }

        /// <summary>
        /// Destroys platform after time spent on the ground
        /// </summary>
        private IEnumerator Destroy()
        {
            yield return new WaitForSeconds(timeTillDestroy);
            Destroy(this.gameObject);
        }

        /// <summary>
        /// Destroys platform after time spent falling if platform is still falling
        /// </summary>
        private IEnumerator DestroyWhenFalling()
        {
            yield return new WaitForSeconds(timeFalling);

            if (isFalling)
            {
                Destroy(this.gameObject);
            }
        }

    }
}