using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallOfFire : MonoBehaviour
{
    // Qualities
    Vector3 _center;
    float _startRadians;
    float _rotationSpeed;
    float _startingRadius;
    float _radiusChangeSpeed;
    float _lifeTime;
    float _speedMultiplier;

    // Tracks start time
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        // this.IntializeBallOfFire(Vector3.zero, 0, 0.3f, 10, -0.1f, 15);
    }

    private void Update()
    {
        if (Time.time - startTime > _lifeTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        // Get how long projectile has been alive
        float timeAlive = Time.time - startTime;

        // Get desired radians and radius
        float desiredRadians = (_startRadians + _rotationSpeed * timeAlive) % 2;
        float desiredRadius = _startingRadius + _radiusChangeSpeed * timeAlive;

        // Calculate desired position
        Vector3 desiredPosition = _center + new Vector3(desiredRadius * Mathf.Cos(desiredRadians * Mathf.PI), desiredRadius * Mathf.Sin(desiredRadians * Mathf.PI), 0);

        // Set velocity toward desired position which increases with distance
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = (desiredPosition - transform.position).normalized * _speedMultiplier * Vector3.Distance(transform.position, desiredPosition);
        if (rigid.velocity.magnitude > 15)
        {
            rigid.velocity = rigid.velocity.normalized * 15;
        }
    }

    // Function used to initialize the projectiles qualities
    public void IntializeBallOfFire(Vector3 center, float startRadians, float rotationSpeed, float startingRadius, float radiusChangeSpeed, float lifeTime, float speedMultiplier)
    {
        _center = center;
        _startRadians = startRadians;
        _rotationSpeed = rotationSpeed;
        _startingRadius = startingRadius;
        _radiusChangeSpeed = radiusChangeSpeed;
        _lifeTime = lifeTime;
        _speedMultiplier = speedMultiplier;

        startTime = Time.time;
    }
}
