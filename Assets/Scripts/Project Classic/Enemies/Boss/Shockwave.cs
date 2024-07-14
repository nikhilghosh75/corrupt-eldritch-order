using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    [SerializeField] float growthRate = 3f;
    [SerializeField] float lifeTime = 3f;
    [SerializeField] float knockbackForce = 10f;
    [SerializeField] float knockbackDuration = 0.5f;

    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        // Destroy shockwave if it's exceeded its lifetime
        if (Time.time - startTime > lifeTime)
        {
            Destroy(this.gameObject);
        }

        // Increase the radius of the shockwave
        Vector3 scale = transform.localScale;
        scale.x += growthRate * Time.deltaTime;
        scale.y += growthRate * Time.deltaTime;
        transform.localScale = scale;
    }

    // Apply force to the player on contact
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerHealth>())
        {
            Rigidbody2D rigid = collision.GetComponent<Rigidbody2D>();
            Vector2 knockbackDirection = (rigid.transform.position - transform.position).normalized;

            // Apply force to player
            PlayerController playerController = collision.GetComponent<PlayerController>();
            playerController.SetController(playerController.knockbackController);
            playerController.DoKnockback(knockbackDuration, knockbackDirection, knockbackForce);
        }
    }
}
