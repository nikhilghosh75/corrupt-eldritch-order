using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WSoft.Math;

public class IsBouncing : MonoBehaviour
{
    public int numBounces = -1;

    int bouncesLeft;
    public bool bounceByDef = true;

    public bool invokeOnDeathOnBounce = false;

    public bool shouldTargetNearbyEnemies = false;
    public LayerMask enemyMask;

    Rigidbody2D rb;

    Vector3 lastImpact;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        bouncesLeft = numBounces;
    }

    private void OnEnable()
    {
        bouncesLeft = numBounces;
    }

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.transform.position == lastImpact) //attempt to eliminate "double bounce" where projectile will impact two overlapping walls and destroy itself rather than bounce
            return;
        else
            lastImpact = gameObject.transform.position;
        // Only bounce off ground and walls
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            other.gameObject.layer == LayerMask.NameToLayer("Wall") ||
            other.gameObject.layer == LayerMask.NameToLayer("Level Bound"))
        {
            // Bounce depending on it being a wall or ground
            if (!rb)
                rb = GetComponent<Rigidbody2D>();
            Vector2 newVelocity = rb.velocity;
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                newVelocity.y *= -1;
            }
            else
            {
                newVelocity.x *= -1;
            }

            if (shouldTargetNearbyEnemies)
            {
                newVelocity = RedirectToEnemy(newVelocity);
            }

            rb.velocity = newVelocity;

            if (bouncesLeft <= 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                bouncesLeft--;
            }

            // Fix the projectile's direction
            // Calculate the angle in radians
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;

            // Create a Quaternion rotation based on the angle (only y-axis rotation)
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

            // Set the rotation instantly
            transform.rotation = targetRotation;
            
            // Invoke on death if true
            if (invokeOnDeathOnBounce)
            {
                GetComponent<Projectile>().onDeath.Invoke();
            }
        }
    }

    private void OnDisable()
    {
        if (!bounceByDef)
            Destroy(this);
    }

    Vector2 RedirectToEnemy(Vector2 direction)
    {
        float angle = Vector2.SignedAngle(Vector2.right, direction);

        for(float f = 0; f < 90f; f += 4f)
        {
            float leftAngle = Mathf.Deg2Rad * (angle - f);
            Vector2 leftDirection = new Vector2(Mathf.Cos(leftAngle), Mathf.Sin(leftAngle));
            RaycastHit2D leftHit = Physics2D.Raycast(transform.position, leftDirection, 40f);

            if (leftHit && leftHit.rigidbody 
                && LayermaskFunctions.IsInLayerMask(enemyMask, leftHit.rigidbody.gameObject.layer))
            {
                Debug.Log("Left Hit " + leftHit.rigidbody.gameObject.name);
                return leftDirection * direction.magnitude;
            }

            float rightAngle = Mathf.Deg2Rad * (angle + f);
            Vector2 rightDirection = new Vector2(Mathf.Cos(rightAngle), Mathf.Sin(rightAngle));
            RaycastHit2D rightHit = Physics2D.Raycast(transform.position, rightDirection, 40f);

            if (rightHit && rightHit.rigidbody 
                && LayermaskFunctions.IsInLayerMask(enemyMask, rightHit.rigidbody.gameObject.layer))
            {
                Debug.Log("Right Hit " + rightHit.rigidbody.gameObject.name);
                return rightDirection * direction.magnitude;
            }
        }

        Debug.Log("No Hit Found" );
        return direction;
    }
}
