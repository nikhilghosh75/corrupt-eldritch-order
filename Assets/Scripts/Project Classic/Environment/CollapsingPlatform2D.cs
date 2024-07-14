/*
 * Makes a platform collapse after the player steps on it.
 * Destroys object after certain time falling or time
 * on ground.
 * @Alex Pohlman, Nikhil Ghosh '24
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WSoft.Math;

public class CollapsingPlatform2D : MonoBehaviour
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

    [Tooltip("The speed at which the platform falls")]
    [SerializeField] float gravityScaleWhenFalling = 6f;

    Rigidbody2D rb;
    bool isShaking = false;
    bool isFalling = false;

    [Tooltip("The layermask to trigger shaking from")]
    [SerializeField] LayerMask layermask;

    [Tooltip("Should we destroy when we fall. If false, we revert the velocity")]
    [SerializeField] bool shouldDestroy;

    [SerializeField] UnityEvent OnCollapseStart;
    [SerializeField] UnityEvent OnCollapseEnd;

    float initialX;
    float initialY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Static;
        rb.freezeRotation = true;

        // Gets platform's initial position
        initialX = transform.position.x;
        initialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Shakes the platform before it begins to fall
        if (isShaking)
        {
            float newX = initialX + Random.Range(-shakeDistance, shakeDistance);
            float newY = initialY + Random.Range(-shakeDistance, shakeDistance);
            Vector3 newPosition = new Vector3(newX, newY, 0);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime * shakeSpeed);
        }
    }

    /// <summary>
    /// Starts shaking and falling when player steps on platform, 
    /// destroys it self after it contacts the ground
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (LayermaskFunctions.IsInLayerMask(layermask, other.gameObject.layer) && !isShaking && !isFalling)
        {
            isShaking = true;
            StartCoroutine(Fall());
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayermaskFunctions.IsInLayerMask(layermask, collision.gameObject.layer) && !isShaking)
        {
            isShaking = true;
            StartCoroutine(Fall());
        }
    }

    /// <summary>
    /// Spends time shaking, then activates gravity and starts falling
    /// </summary>
    private IEnumerator Fall()
    {
        OnCollapseStart.Invoke();
        yield return new WaitForSeconds(timeTillCollapse);
        isShaking = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = gravityScaleWhenFalling;
        rb.velocity = Vector2.zero;
        isFalling = true;
        StartCoroutine(DestroyWhenFalling());
    }

    /// <summary>
    /// Destroys platform after time spent falling if platform is still falling
    /// </summary>
    private IEnumerator DestroyWhenFalling()
    {
        yield return new WaitForSeconds(timeFalling);

        if (isFalling)
        {
            OnCollapseEnd.Invoke();
            if (shouldDestroy)
            {
                Destroy(this.gameObject);
            }
            else
            {
                transform.position = new Vector3(initialX, initialY, transform.position.z);
                rb.bodyType = RigidbodyType2D.Static;
                isFalling = false;
            }
        }
    }

}