using System.Collections;
using Unity.VisualScripting;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerInputType
{
    Jump,
    Dash,
    Attack,
    Freeze,
}

[System.Serializable]
public abstract class PlayerSubController
{
    [HideInInspector]
    public PlayerController playerController;

    public string name { get; protected set; }

    /// <summary>
    /// Called when this subcontroller is getting initialized when the player starts. You can assume the player controller is initialized.
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// Called when this subcontroller becomes the new subcontroller of the character
    /// </summary>
    public abstract void OnEnable();

    /// <summary>
    /// Called when this subcontroller stops being the new subcontroller of the character
    /// </summary>
    public abstract void OnDisable();

    /// <summary>
    /// Called when the subcontroller is the current subcontroller of the character
    /// </summary>
    public abstract void FixedUpdate();

    /// <summary>
    /// Called when the subcontroller is the current subcontroller of the character and the player attempts to move
    /// </summary>
    public abstract void Move(InputAction.CallbackContext context);

    /// <summary>
    /// Called when the subcontroller receives input from the player controller. Only called when it is the current subcontroller.
    /// </summary>
    /// <param name="type"></param>
    public abstract void RecieveInput(PlayerInputType type);
}

public class PlayerController : MonoBehaviour
{
    [Header("Player Subcontrollers")]
    public PlayerGroundController groundController;
    public PlayerAirController airController;
    public PlayerDashController dashController;
    public PlayerWallController wallController;
    public PlayerKnockbackController knockbackController;

    [Header("Player Components")]
    public Rigidbody2D rb;
    public PlayerWeapon playerWeapon;
    private PlayerPowerupManager powerupManager;
    public PlayerMana playerMana;
    public float adaptableJumpGravFactor = 0.5f;
    public AnimationCharacterController Character_Anim;
    public PlayerInput actionAsset;

    public float horizontal;
    public float vertical;
    public Transform groundCheck;
    public Transform wallCheck;
    public Transform wallCheckBehind;
    public Transform ceilingCheck;
    public LayerMask groundLayer;
    public LayerMask powerupLayer;
    public LayerMask wallLayer;
    public InputActionReference moveAction;

    [Header("Player Stats")]
    [SerializeField]
    public PlayerHealth playerHealth;
    public bool droppingThroughPlatform = false;
    public bool paused = false;

    [Serialize]
    public bool strafingOn = true;
    public Vector2 aimDirection;
    public bool aimLocked = false;
    public bool continuousAimingOn;
    private bool mouseActive;
    private Vector2 mousePosition;
    private Vector2 mouseWorldPoint;

    [SerializeField]
    public PlayerSubController currentController { get; private set; }

    public static PlayerController Instance { get; private set; }

    [SerializeField]
    private bool shootWeakWeapon, shootStrongWeapon, shootComboWeapon;
    [SerializeField]
    private bool stayingInPlace = false;
    [SerializeField]
    private bool frozen = false;
    public bool debuffed = false;

    float originalGravityScale;
    private void Awake()
    {
        groundController.playerController = this;
        groundController.Initialize();
        airController.playerController = this;
        airController.Initialize();
        dashController.playerController = this;
        dashController.Initialize();
        wallController.playerController = this;
        wallController.Initialize();
        knockbackController.playerController = this;
        knockbackController.Initialize();

        playerHealth = GetComponent<PlayerHealth>();
        powerupManager = GetComponent<PlayerPowerupManager>();
        playerMana = GetComponent<PlayerMana>();

        Instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        actionAsset = GetComponent<PlayerInput>();

        SetController(groundController);

        RunManager.Instance?.ApplyPermanentUpgrades(powerupManager);
    }

    private void OnEnable()
    {
        Time.timeScale = 1;
        currentController?.RecieveInput(PlayerInputType.Jump);

    }

    void FixedUpdate()
    {
        // Shoot
        if (shootWeakWeapon)
            playerWeapon.WeakAttack();
        if (shootStrongWeapon)
            playerWeapon.StrongAttack();
        if (shootComboWeapon)
            playerWeapon.ComboAttack();

        currentController.FixedUpdate();

        if (continuousAimingOn && mouseActive)
        {
            mouseWorldPoint = Camera.main.ScreenToWorldPoint(mousePosition);
            aimDirection = (mouseWorldPoint - (Vector2)transform.position).normalized;
            if (aimDirection.x != 0)
            {
                if (currentController == wallController && !wallController.isWallOnRight)
                {
                    aimDirection = new Vector2(math.abs(aimDirection.x), aimDirection.y);
                }
                else if (currentController == wallController)
                {
                    aimDirection = new Vector2(-math.abs(aimDirection.x), aimDirection.y);
                }
                transform.localScale = new Vector3((aimDirection.x < 0 ? Mathf.Abs(transform.localScale.x) * -1.0f : Mathf.Abs(transform.localScale.x) * 1.0f), transform.localScale.y, 1.0f);
            }
        }
    }

    public void SetController(PlayerSubController newController)
    {
        if (currentController != null)
            currentController.OnDisable();

        currentController = newController;
        currentController.OnEnable();
    }

    public void SetControllerWithDelay(PlayerSubController newController, float delayTime)
    {
        StartCoroutine(DelayedSetController(newController, delayTime));
    }

    IEnumerator DelayedSetController(PlayerSubController controller, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SetController(controller);
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public Transform GetGround()
    {
        if (IsGrounded())
        {
            return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer).transform;
        }
        return null;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (stayingInPlace) return;
        if (frozen) return;

        if (context.started)
        {
            currentController.RecieveInput(PlayerInputType.Jump);
            originalGravityScale = rb.gravityScale;
            rb.gravityScale *= adaptableJumpGravFactor;
        }
        if(context.canceled)
        {
            // Don't change the gravity when we are on the ceiling
            if (transform.localScale.y > 0)
            {
                rb.gravityScale = originalGravityScale;
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (stayingInPlace || frozen)
        {
            horizontal = 0;
            return;
        } 
        currentController.Move(context);
    }

    public void Aim(InputAction.CallbackContext context)
    {
        if (continuousAimingOn)
        {
            return;
        }
        if (context.action.name != "Aim" && actionAsset.currentActionMap.FindAction("Aim").IsPressed())
        {
            aimDirection = actionAsset.currentActionMap.FindAction("Aim").ReadValue<Vector2>().normalized;
            if (context.control.device is Gamepad)
            {
                aimDirection = RoundToNearest45(aimDirection);
            }
            else if (SettingsLoader.Settings.eightDirectionalEnabled)
            {
                aimDirection = RoundToNearest45(aimDirection);
            }
        }
        else if (!strafingOn || !aimLocked)
        {
            aimDirection = context.ReadValue<Vector2>().normalized;
            if (context.control.device is Gamepad)
            {
                aimDirection = RoundToNearest45(aimDirection);
            }
            else if (SettingsLoader.Settings.eightDirectionalEnabled)
            {
                aimDirection = RoundToNearest45(aimDirection);
            }
        }
    }

    public void ContinuousAim(InputAction.CallbackContext context)
    {
        if (continuousAimingOn && context.control.device is Mouse)
        {
            // mouse input is supported here 
            mousePosition = context.ReadValue<Vector2>();
            mouseActive = true;
            mouseWorldPoint = Camera.main.ScreenToWorldPoint(mousePosition);
        }
        else if (continuousAimingOn)
        {
            // controller input is supported here
            aimDirection = context.ReadValue<Vector2>().normalized;
            mouseActive = false;
            if (aimDirection.x != 0)
            {
                transform.localScale = new Vector3((aimDirection.x < 0 ? Mathf.Abs(transform.localScale.x) * -1.0f : Mathf.Abs(transform.localScale.x) * 1.0f), transform.localScale.y, 1.0f);
            }
        }
    }

    public void DetectControl(InputAction.CallbackContext context){
        //Auto switch to mouse aiming if mouse buttons are pressed
        if (context.control.device is not Mouse && context.control.device is not Keyboard){
            return;
        }
        if (SettingsLoader.Settings.eightDirectionalEnabled)
        {
            continuousAimingOn = false;
        }
        else
        {
            if (context.control.device is Keyboard)
            {
                continuousAimingOn = false;
                return;
            }
            if (context.control.device is Mouse)
            {
                continuousAimingOn = true;
                return;
            }
        }
    }

    public void WeakAttack(InputAction.CallbackContext context)
    {
        if (frozen) return;

        if (context.performed){
            shootWeakWeapon = true;
            aimLocked = true;
        }
        else if (context.canceled){
            shootWeakWeapon = false;
            aimLocked = false;
            Aim(context);
        }
    }

    public void StrongAttack(InputAction.CallbackContext context)
    {
        if (frozen) return;

        if (context.performed){
            shootStrongWeapon = true;
            aimLocked = true;
        }
        else if (context.canceled){
            shootStrongWeapon = false;
            aimLocked = false;
            Aim(context);
        }
    }

    public void ComboAttack(InputAction.CallbackContext context)
    {
        if (frozen || debuffed) return;
        // If continuous aming is on, and both buttons are held, fire combo instead
        if(continuousAimingOn){
            if (context.performed){
                shootComboWeapon = true;
                aimLocked = true;
                shootWeakWeapon = false;
                shootStrongWeapon = false;
            }
            else if (context.canceled){
                shootComboWeapon = false;
                aimLocked = false;
                Aim(context);
            }
        }
        // Using normal context keyboard should still work
        if (context.performed){
            shootComboWeapon = true;
            aimLocked = true;
        }
        else if (context.canceled){
            shootComboWeapon = false;
            aimLocked = false;
            Aim(context);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (frozen || debuffed) return;

        if (context.started)
            currentController.RecieveInput(PlayerInputType.Dash);
    }

    public void Freeze(InputAction.CallbackContext context)
    {
        if (!IsGrounded()) 
        {
            stayingInPlace = false;
            horizontal = 0;
            return;
        }
        if (context.started)
        {
            stayingInPlace = true;
            horizontal = 0;
        }
        if (context.canceled){
            stayingInPlace = false;
            horizontal = aimDirection.x;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Collider2D col = Physics2D.OverlapCircle(transform.position, powerupManager.pickupRange, powerupLayer);

            if (col != null && col.TryGetComponent(out PowerupTrigger powerup) == true)
            {
                if (powerup.ApplyPowerup(powerupManager))
                    Destroy(col.gameObject);
                else // This should be removed ASAP, but for some reason weapon powerups will not be destroyed on collection due to ApplyPowerup not returning true. This counteracts that but is a very bad long-term solution.
                {
                    powerup.gameObject.GetComponent<SpriteRenderer>().sprite = null;
                    powerup.gameObject.GetComponent<PowerupTrigger>().enabled = false;
                }
            }
            else if (col != null && col.TryGetComponent(out ShopTrigger shop) == true)
            {
                shop.Purchase(powerupManager);
            }
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (!paused)
        {
            EventBus.Publish(new PauseEvent());
            paused = true;
        }
        else 
        {
            EventBus.Publish(new ResumeEvent());
            paused = false;
        }
    }

    public void DisablePlayer()
    {
        frozen = true;
        horizontal = 0;
        vertical = 0;

        // Also disable weapon firing if it's happening
        shootWeakWeapon = false;
        shootStrongWeapon = false;
        shootComboWeapon = false;
    }

    public void EnablePlayer()
    {
        frozen = false;
    }

    private static Vector2 RoundToNearest45(Vector2 vector)
    {
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        int octant = Mathf.RoundToInt(angle / 45f);
        float snappedAngle = octant * 45f * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(snappedAngle), Mathf.Sin(snappedAngle));
    }

    // It is very important that this function is called instead of starting a coroutine from an enemy MonoBehaviour
    public void DoKnockback(float maxDuration, Vector2 direction, float force)
    {
        StartCoroutine(knockbackController.ApplyKnockback(maxDuration, direction, force));
    }
}
