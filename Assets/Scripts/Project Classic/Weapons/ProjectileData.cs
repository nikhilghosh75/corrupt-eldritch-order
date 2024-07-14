using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Project Classic/Projectile")]
public class ProjectileData : ScriptableObject
{
    [Header("Projectile Visuals")]
    [Tooltip("Name of projectile")]
    [SerializeField] string _projectileName = "";
    public string projectileName
    {
        get
        {
            return _projectileName;
        }
        private set
        {
            _projectileName = value;
        }
    }
    [Tooltip("Sprite used for projectile")]
    [SerializeField] Sprite _sprite = null;
    public Sprite sprite
    {
        get
        {
            return _sprite;
        }
        private set
        {
            _sprite = value;
        }
    }

    [Header("Projectile Stats")]
    [Tooltip("Damage")]
    public int damage = 1;
    [Tooltip("Speed of projectile")]
    public float speed = 5f;
    [Tooltip("Time until projectile is destroyed")]
    public float timeUntilDestroy = 3f;
    [Tooltip("Number of time projectile will pierce enemies")]
    public int pierce = 0;
    [Tooltip("Whether or not projectile belongs to the player")]
    public bool belongsToPlayer = true;
    [Tooltip("Scale projectile in the x direction")]
    public float scaleX = 1;
    [Tooltip("Scale projectile in the y direction")]
    public float scaleY = 1;
    [Tooltip("Collider Radius")]
    public float colliderRadius = 0.25f;

    [Header("Explosive")]
    [Tooltip("If true, the projectile will be explosive")]
    public bool isExplosive = false;
    [Tooltip("Explosion prefab spawned on explosion")]
    public List<GameObject> explosionPrefabs;
    [Tooltip("Radius of explosion")]
    public List<float> explosionRadii;

    [Header("Bouncing")]
    [Tooltip("If true, the projectile will be bouncing")]
    public bool isBouncing = false;
    [Tooltip("Controls the number of times this will bounce")]
    public int numBounces = 1;
    [Tooltip("If true, on death event will be invoked when this projectile bounces")]
    public bool invokeOnDeathOnBounce = false;

    [Header("Lobbed")]
    [Tooltip("If true, the projectile will be lobbed")]
    public bool isLobbed = false;
    [Tooltip("Force of gravity on this projectile")]
    public float gravityScale = 2f;

    [Header("Burning")]
    [Tooltip("If true, projectile will cause burn")]
    public bool isBurning = false;

    [Header("Freezing")]
    [Tooltip("If true, the projectile will be freezing")]
    public bool isFreezing = false;

    [Header("Homing")]
    [Tooltip("If true, the projectile will be homing")]
    public bool isHoming = false;
}
