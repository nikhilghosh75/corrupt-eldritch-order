using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager instance { get; private set; }

    public int jumpCount = 0;
    public int enemiesKilled = 0;
    public int bossDefeatedCount = 0;
    public int score = 0;
    public int permanentCurrencyAcquired = 0;
    public bool beatenGame = false;

    public Dictionary<string, int> damagedBy = new Dictionary<string, int>();
    public Dictionary<EnemyType, int> enemyTypesKilled = new Dictionary<EnemyType, int>();
    public string lastDamagedBy = "";

    [Header("Scores")]
    [SerializeField] int killNormalEnemyScore;
    [SerializeField] int clearARoomScore;
    [SerializeField] int perfectlyClearARoomScore;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        EventBus.Subscribe<JumpEvent>(OnJumpUsed);
        EventBus.Subscribe<EnemyKilledEvent>(OnEnemyKilled);
        EventBus.Subscribe<RoomClearEvent>(OnRoomCleared);
        EventBus.Subscribe<BossDefeatedEvent>(OnBossDefeated);
        EventBus.Subscribe<CurrencyAddedEvent>(OnCurrencyAdded);
    }

    public void SetFromSaveData(ProgressionSaveData data)
    {
        jumpCount = data.jumpCount;
        enemiesKilled = data.enemiesKilledCount;
        bossDefeatedCount = data.bossDefeatedCount;
        permanentCurrencyAcquired = data.permanentCurrencyAcquired;

        foreach (DamagedByStatSaveData saveData in data.damagedBy)
        {
            if (damagedBy.ContainsKey(saveData.damagedBy))
            {
                damagedBy[saveData.damagedBy] = saveData.numTimesDamagedBy;
            }
            else
            {
                damagedBy.Add(saveData.damagedBy, saveData.numTimesDamagedBy);
            }
        }

        enemyTypesKilled.Add(EnemyType.Soldier, data.soldiersKilledCount);
        enemyTypesKilled.Add(EnemyType.Drone, data.dronesKilledCount);
        enemyTypesKilled.Add(EnemyType.Infestation, data.infestationKilledCount);
        enemyTypesKilled.Add(EnemyType.Debuffer, data.debufferKilledCount);
        enemyTypesKilled.Add(EnemyType.Warlock, data.warlockKilledCount);
    }

    public void SaveProgressionData(ref ProgressionSaveData data)
    {
        data.jumpCount = jumpCount;
        data.enemiesKilledCount = enemiesKilled;
        data.bossDefeatedCount = bossDefeatedCount;
        data.permanentCurrencyAcquired = permanentCurrencyAcquired;

        data.damagedBy = new DamagedByStatSaveData[damagedBy.Count];
        int i = 0;
        foreach(KeyValuePair<string, int> pair in damagedBy)
        {
            data.damagedBy[i] = new DamagedByStatSaveData(pair.Key, pair.Value);
            i++;
        }
    }

    public void RecordDamage(string damageSource)
    {
        lastDamagedBy = damageSource;
        if (damagedBy.ContainsKey(damageSource))
        {
            damagedBy[damageSource] = damagedBy[damageSource] + 1;
        }
        else
        {
            damagedBy.Add(damageSource, 1);
        }
    }

    void OnJumpUsed(JumpEvent e)
    {
        jumpCount++;
    }

    void OnEnemyKilled(EnemyKilledEvent e)
    {
        enemiesKilled++;
        score += killNormalEnemyScore;

        EnemyData data = e.enemy.GetComponent<EnemyData>();
        if (data == null)
        {
            return;
        }

        EnemyType type = data.type;
        if (enemyTypesKilled.ContainsKey(type))
        {
            enemyTypesKilled[type] += 1;
        }
        else
        {
            enemyTypesKilled.Add(type, 1);
        }
    }

    void OnRoomCleared(RoomClearEvent e)
    {
        if (e.perfectClear)
        {
            score += perfectlyClearARoomScore;
        }
        else
        {
            score += clearARoomScore;
        }
    }

    void OnBossDefeated(BossDefeatedEvent e)
    {
        bossDefeatedCount++;
    }

    void OnCurrencyAdded(CurrencyAddedEvent e)
    {
        permanentCurrencyAcquired += e.addedPermanentCurrency;
    }
}
public class JumpEvent
{

}

public class DashEvent
{

}

public class EnemyKilledEvent
{
    public EnemyHealth enemy;
    
    public EnemyKilledEvent(EnemyHealth _enemy)
    {
        enemy = _enemy;
    }
}

public class RoomClearEvent
{
    public bool perfectClear;

    public RoomClearEvent(bool perfect)
    {
        perfectClear = perfect;
    }
}

public class GameCompleteEvent
{

}