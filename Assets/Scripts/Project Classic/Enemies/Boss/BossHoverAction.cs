using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHoverAction : EnemyMoveAction
{
    // Relative Hover point
    Vector3 positionRelativeToPlayer;

    // Player reference
    PlayerHealth player;

    // Rigid Body
    Rigidbody2D rb;

    // Boss Animator
    [SerializeField] BossAnimationController animationController;

    // Hover time
    [SerializeField] public float hoverTime = 2f;

    // Hooke's Law Shit
    [Header("Hooke's Law")]
    [SerializeField] public float constant = 1f;
    [SerializeField] float dampeningFactor = 0.9f;
    [SerializeField] public float maxSpeed = 10f;

    // Sin bobbing
    [Header("Wave-Like Motion")]
    [SerializeField] float amplitude;
    [SerializeField] float period;

    Coroutine hoverCoroutine;

    public override void Act()
    {
        positionRelativeToPlayer = GetComponent<BossBehavior>().currentRelativePosition;
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        hoverCoroutine = StartCoroutine(HoverCoroutine());
    }

    IEnumerator HoverCoroutine()
    {
        float startTime = Time.time;
        GameObject center = FindObjectOfType<BossTrigger>().bossRoomCenter;
        Vector3 centerPosition = center.transform.position + positionRelativeToPlayer;
        animationController.Moving = true;

        while (Time.time - startTime < hoverTime)
        {
            Vector3 hoverPosition = centerPosition + Vector3.up * amplitude * Mathf.Sin(Time.time / (2 * Mathf.PI) * period);

            UpdateOrientation();

            Vector3 currentVelocity = rb.velocity;

            // Find how far transform is from its target position
            Vector3 deltaPosition = hoverPosition - transform.position;

            // Apply hookes law
            currentVelocity = dampeningFactor * (currentVelocity + (deltaPosition * constant * 0.1f));
            if (currentVelocity.magnitude > maxSpeed)
            {
                currentVelocity = currentVelocity.normalized * maxSpeed;
            }
            rb.velocity = currentVelocity;

            yield return new WaitForSeconds(0.1f);
        }

        rb.velocity = Vector3.zero;
        animationController.Moving = false;
    }

    public override float GetActionTime()
    {
        return hoverTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerHealth>();
        positionRelativeToPlayer = transform.position - player.transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Interrupt()
    {
        StopCoroutine(hoverCoroutine);
        rb.velocity = Vector3.zero;
    }

    void UpdateOrientation()
    {
        float distance = player.transform.position.x - transform.position.x;
        if(distance < -1)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (distance > 1)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}
