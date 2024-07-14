using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WSoft.Combat;

public class BossBehavior : EnemyBehavior
{
    // Paramaters for tweaking boss behavior
    [Header("Number of attacks")]
    [SerializeField] int minAttacksBeforeMove = 1;
    [SerializeField] int maxAttacksBeforeMove = 3;

    // Boss Actions
    [Header("Action References")]
    [SerializeField] public EnemyMoveAction hoverAction;
    [SerializeField] public  EnemyMoveAction changePositionAction;
    [SerializeField] List<EnemyFireProjectileAction> enemyAttackActions;

    // Contact Damage
    [Header("Contact Damage")]
    [SerializeField] int contactDamage;

    // Boss's current relative position
    public Vector3 currentRelativePosition;

    // Room Transform
    GameObject roomTransform;

    // Reference to the boss's mode
    [SerializeField] List<BossMode> activeBossModes = new();

    private void Awake()
    {
        GetComponent<HealthSystem>().events.OnDeath.AddListener(OnBossDefeated);
    }

    protected override IEnumerator EnemyRoutine()
    {
        // Randomly select a boss mode
        SelectRandomModes();
        ChooseBossModes();

        // Get only active boss attacks
        EnemyFireProjectileAction[] possibleAttackOptions = GetComponents<EnemyFireProjectileAction>();
        foreach (EnemyFireProjectileAction a in possibleAttackOptions)
        {
            if (a.enabled)
            {
                enemyAttackActions.Add(a);
            }
        }

        // Save attack ids to the run manager
        int[] ids = new int[enemyAttackActions.Count];
        for (int i = 0; i < enemyAttackActions.Count; i++)
        {
            // Assign the 'id' from the component to the array
            ids[i] = enemyAttackActions[i].id;
        }
        RunManager.Instance.currentAttackIDs = ids;


        foreach (BossMode bossMode in activeBossModes)
        {
            bossMode.OnBehaviorStart();
        }

        while (true)
        {
            if (changePositionAction) yield return ExecuteActionUntilFinished(changePositionAction);

            int movesBeforeAttack = Mathf.FloorToInt(Random.Range(minAttacksBeforeMove, maxAttacksBeforeMove + 1));

            for (int i=0; i<movesBeforeAttack; ++i)
            {
                if(hoverAction) yield return ExecuteAction(hoverAction);

                if (enemyAttackActions.Count == 0) continue;
                int attackSelection = Mathf.FloorToInt(Random.Range(0, enemyAttackActions.Count));
                yield return ExecuteAction(enemyAttackActions[attackSelection]);

                // Post attack action
                foreach (BossMode bossMode in activeBossModes)
                {
                    bossMode.OnPostAttack();
                }
            }

            yield return null;
        }
    }

    // Damages player on contact
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().Damage(contactDamage);
        }
    }

    public GameObject GetRoomObject()
    {
        if (roomTransform)
        {
            return roomTransform;
        }

        // Get all the rooms in the scene
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Level");

        // Find the closest one, this will be the one the boss is in
        GameObject room = null;
        float longestDistance = 1000000f;
        foreach (GameObject r in rooms)
        {
            float distance = Vector3.Distance(transform.position, r.transform.position);
            if (distance < longestDistance)
            {
                room = r;
                longestDistance = distance;
            }
        }

        return roomTransform = room;
    }

    // Enables numModes modes for boss to use in fight
    public void SelectRandomModes()
    {
        BossMode[] bossModes = GetComponents<BossMode>();
        LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();
        // Enable modes that are valid for that level that have not previously been enabled
        BossMode[] validBossModes = bossModes.Where(mode => levelGenerator.bossModes.Contains(mode.ModeID) &&
                                                            !RunManager.Instance.currentModeIDs.Contains(mode.ModeID)).ToArray();

        int numModes = levelGenerator.bossModeCount;

        for (int i = 0; i < bossModes.Length; i++)
        {
            // Only disable this mode if it's not stored in the run manager
            if (!RunManager.Instance.currentModeIDs.Contains(bossModes[i].ModeID))
            {
                bossModes[i].enabled = false;
            }
        }

        // Shuffle array
        ShuffleArray<BossMode>(validBossModes);
        int[] newModeIDs = new int[numModes - RunManager.Instance.currentModeIDs.Length];
        // Enable first numModes minus previously enabled modes and disable the rest 
        for (int i=0; i < validBossModes.Length; ++i)
        {
            if (i<numModes - RunManager.Instance.currentModeIDs.Length)
            {
                validBossModes[i].enabled = true;
                newModeIDs[i] = validBossModes[i].ModeID;
            }
        }

        // Append previous modes
        int[] modeIDs = new int[numModes];
        RunManager.Instance.currentModeIDs.CopyTo(modeIDs, 0);
        newModeIDs.CopyTo(modeIDs, RunManager.Instance.currentModeIDs.Length);
        
        BossName bossName = FindObjectOfType<BossName>();
        bossName.InitializeName(modeIDs);

        // Save new modes
        RunManager.Instance.currentModeIDs = modeIDs;
    }

    // Enables numAttacks attacks for boss to use in fight
    public void SelectRandomAttacks()
    {
        EnemyFireProjectileAction[] allAttacks = GetComponents<EnemyFireProjectileAction>();

        // removes all attacks that are stored in the run manager
        EnemyFireProjectileAction[] attacks = allAttacks.Where(attack => !RunManager.Instance.currentAttackIDs.Contains(attack.id)).ToArray();

        // Shuffles array
        ShuffleArray<EnemyFireProjectileAction>(attacks);

        // Get num attacks from level config
        LevelGenerator levelGenerator = FindFirstObjectByType<LevelGenerator>();
        int numAttacks = levelGenerator.levelConfig.enemyItems.bossAttackCount;

        // Enable first numAttacks minus previously enabled attacks and disable the rest
        for (int i = 0; i < attacks.Length; ++i)
        {
            if (i<(numAttacks - RunManager.Instance.currentAttackIDs.Length))
            {
                attacks[i].enabled = true;
            }
            else
            {
                attacks[i].enabled = false;
            }
        }
    }

    private void ShuffleArray<T>(T[] array)
    {
        // Fisher-Yates shuffle
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            int rnd = Random.Range(i, n);
            T temp = array[rnd];
            array[rnd] = array[i];
            array[i] = temp;
        }
    }

    private void ChooseBossModes()
    {
        // Randomly select a boss mode
        BossMode[] allBossModes = GetComponents<BossMode>();
        foreach (BossMode bossMode in allBossModes)
        {
            if (bossMode.enabled)
            {
                activeBossModes.Add(bossMode);
            }
        }
    }

    public void OnBossDefeated()
    {
        // Destroy all projectiles
        Projectile[] projectiles = GameObject.FindObjectsOfType<Projectile>();
        foreach (Projectile projectile in projectiles)
        {
            if (projectile.gameObject.name == "NEW PROJECTILE")
            {
                projectile.gameObject.SetActive(false);
            }
        }

        // Stop all boss attacks (freezing the boss is the most convienent way, it will be destroyed soon anyway)
        EventBus.Publish(new BossDefeatedEvent());
        Freeze(10000);
    }
}
