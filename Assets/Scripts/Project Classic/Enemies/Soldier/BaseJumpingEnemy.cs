using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseJumpingEnemy : EnemyBehavior
{
    [Header("General")]
    public string damageSourceName;
    public float speed = 3.0f;

    [Header("Audio")]
    public AK.Wwise.Event meleeSFX;
    public AK.Wwise.Event jumpSFX;

    [Header("Jumping")]
    public float jumpDuration = 2.0f;
    public float jumpTime = 1.0f;
    public float jumpDelay = 1.5f;
    public float jumpCooldown = 1.0f;
    public float jumpArcHeight = 1.5f;
    public float maxJumpDistance = 20f;

    [Header("Knockback")]
    public float knockbackDuration = 0.25f;
    public Vector2 knockbackDirection;
    public float knockbackForce = 30.0f;
    public int knockbackDamage = 5;

    [Header("Ground Utility")]
    public Transform groundChecker;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    public bool canDamage = false;
    public int damageDirection;

    //General
    private GameObject target;
    private PlatformEffector2D[] platforms;
    private SpriteRenderer enemyRenderer;
    protected LevelEnemyManager enemyManager;
    protected Rigidbody2D rb;
    protected Animator anim;

    private bool active = false;
    protected bool frozen = false;
    protected bool canMove = true;
    protected bool canAct = true;
    protected EnemyHealth healthData;

    protected virtual void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        platforms = FindObjectsOfType<PlatformEffector2D>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        wallLayer = LayerMask.GetMask("Wall");
        healthData = GetComponent<EnemyHealth>();

        enemyManager = transform.parent.GetComponent<LevelEnemyManager>();
        if (!enemyManager)
        {
            foreach (Transform child in transform.parent)
            {
                if (child.GetComponent<LevelEnemyManager>())
                {
                    enemyManager = child.GetComponent<LevelEnemyManager>();
                }
            }
        }
    }

    void Update()
    {
        if (canDamage)
        {
            if (CheckPlayerCollision())
            {
                OnPlayerOverlap(damageDirection);
            }
        }

        bool isMoving = Mathf.Abs(rb.velocity.x) > 1;
        anim.SetBool("Moving", active && isMoving);
    }

    protected void Move()
    {
        //move
        if (Mathf.Abs(DistanceToPlayerX()) < .2f) {
            return;
        }
        Vector2 velocity = rb.velocity;
        velocity.x = (transform.localScale.x/Mathf.Abs(transform.localScale.x)) * speed;
        rb.velocity = velocity;

        //TODO: potential future change
        //if going to fall off, jump?
    }

    //BEGIN JUMPING LOGIC
    protected IEnumerator Jump()
    {
        //if (!IsGroundAhead()) yield break;

        jumpSFX?.Post(gameObject);
        PlatformEffector2D target_platform = FindTargetPlatform();
        //if there is no platform, return
        if (target_platform == null) yield break;

        Vector2 jump_target = FindSpotOnPlatform(target_platform);

        //ensure that the npc does not do anything during its jump
        canAct = false;

        Vector2 jump_velocity = CalculateJumpVelocity(transform.position, jump_target);

        rb.velocity = Vector2.zero;
        rb.velocity = jump_velocity;
        
        yield return new WaitForSeconds(jumpTime);
        rb.velocity = Vector2.zero;

        canMove = true;
        yield return new WaitForSeconds(jumpCooldown);
        canAct = true;
    }

    private IEnumerator FreezeTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        frozen = false;
    }

    private PlatformEffector2D FindTargetPlatform()
    {
        //TODO: restrict platforms to current room? update platforms every time new room is reached

        PlatformEffector2D next_platform = null;

        for (int i = 0; i < platforms.Length; i++)
        {
            Vector3 platform_pos = platforms[i].transform.position;

            //if the platform is in the same direction as the player relative to the soldier
            if (
                (((target.transform.position - transform.position).x < 0 &&
                (platform_pos - transform.position).x < 0) ||
                ((target.transform.position - transform.position).x > 0 &&
                (platform_pos - transform.position).x > 0))
                &&
                (((target.transform.position - transform.position).y < 0 &&
                (platform_pos - transform.position).y < 0) ||
                ((target.transform.position - transform.position).y > 0 &&
                (platform_pos - transform.position).y > 0))
                )
            {
                RaycastHit2D hit = Physics2D.Linecast(transform.position, platform_pos, groundLayer);
                if(hit.collider != null && hit.collider.gameObject.GetComponent<PlatformEffector2D>() == null)
                {
                    //if there is a collider in between the enemy and the platform, dont jump
                    continue;
                }
                if(Vector2.Distance(transform.position, platform_pos) > maxJumpDistance)
                {
                    //if the platform is too far, dont consider it
                    continue;
                }
                //find the closest platform to the soldier
                if (next_platform == null)
                {
                    next_platform = platforms[i];
                }
                else if (Vector2.Distance(transform.position, platform_pos) <
                    Vector2.Distance(transform.position, next_platform.gameObject.transform.position))
                {
                    next_platform = platforms[i];
                }
            }
        }

        return next_platform;
    }

    private Vector2 FindSpotOnPlatform(PlatformEffector2D platform)
    {
        Vector2 result = new();

        //spot to jump to will be the y pos of the platform + platform thickness + enemy height offset
        result.y =
            platform.gameObject.transform.position.y +
            (platform.GetComponent<BoxCollider2D>().bounds.extents.y / 2) +
            (gameObject.GetComponent<BoxCollider2D>().bounds.extents.y / 2);

        //choose a random x coordinate on the platform to jump to
        float xmin =
            platform.gameObject.transform.position.x -
            (platform.GetComponent<BoxCollider2D>().bounds.extents.x / 2);
        float xmax =
            platform.gameObject.transform.position.x +
            (platform.GetComponent<BoxCollider2D>().bounds.extents.x / 2);
        result.x = Random.Range(xmin, xmax);

        return result;
    }

    private Vector2 CalculateJumpVelocity(Vector2 start_pos, Vector2 end_pos)
    {
        //TODO: potentially adjust jump time based on length of jump
         float vx = (end_pos.x - start_pos.x) / jumpTime;

        float g = Mathf.Abs(Physics2D.gravity.y) * rb.gravityScale;
        float vy = ((end_pos.y + jumpArcHeight - start_pos.y) / jumpTime) + (0.5f * g * jumpTime);

        return new Vector2(vx, vy);
    }
    //END JUMPING LOGIC

    //BEGIN COLLISION LOGIC
    protected bool CheckPlayerCollision()
    {
        if(healthData.isDying) {
            return false;
        }
        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        filter.SetLayerMask(LayerMask.GetMask("Player"));
        filter.useLayerMask = true;

        Collider2D[] results = new Collider2D[1];

        return GetComponent<BoxCollider2D>().OverlapCollider(filter, results) > 0;
    }

    protected void OnPlayerOverlap(int knockback_direction)
    {
        if (healthData.isDying)
        {
            return;
        }
        //TODO: activate player iframes
        //as of now knockback is angled 45 degrees away from enemy
        //changed to more than 45 degrees

        knockbackDirection.x = knockback_direction;
        knockbackDirection.y = 1.5f;
        PlayerController playerController = target.GetComponent<PlayerController>();
        playerController.SetController(playerController.knockbackController);
        playerController.DoKnockback(knockbackDuration, knockbackDirection, knockbackForce);
        target.GetComponent<PlayerHealth>().Damage(knockbackDamage);

        meleeSFX?.Post(gameObject);
        canDamage = false;

        ProgressionManager.instance.RecordDamage(damageSourceName);
    }
    //END COLLISION LOGIC

    protected bool IsGroundAhead()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundChecker.position, Vector2.down, 0.1f, groundLayer);
        return hit.collider != null;
    }

    protected bool IsWallAhead()
    {
        RaycastHit2D wallHit = Physics2D.Raycast(groundChecker.position, transform.forward, 0.1f, wallLayer);
        return wallHit.collider != null;
    }

    protected void UpdateOrientation()
    {
        //TODO: if stacked above player, path off of current platform
        //if stacked above player, prevent rapid turning
        if ((Mathf.Abs(DistanceToPlayerX()) < 5 && Mathf.Abs(DistanceToPlayerY()) > 1)) return;

        if ((DistanceToPlayerX() > 1.5 && transform.parent.localScale.x > 0) ||
                (DistanceToPlayerX() < -1.5 && transform.parent.localScale.x < 0))//if player on the left
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if ((DistanceToPlayerX() < -1.5 && transform.parent.localScale.x > 0) ||
                (DistanceToPlayerX() > 1.5 && transform.parent.localScale.x < 0))
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

    }

    protected float DistanceToPlayerX()
    {
        return target.transform.position.x - transform.position.x;
    }

    protected float DistanceToPlayerY()
    {
        return target.transform.position.y - transform.position.y;
    }

    protected bool IsPlayerGrounded()
    {
        PlayerController pc = target.GetComponent<PlayerController>();
        return pc.IsGrounded();
    }

    public void Death()
    {
        canMove = false;
        canAct = false;
        active = false;
        StopAllCoroutines();
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void EnableActing()
    {
        canMove = false;
    }
}
