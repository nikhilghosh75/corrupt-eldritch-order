using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Combat;

/* bound to change by design, placeholders */
public enum EnemyType
{
    Soldier, Drone, Infestation, Debuffer, Warlock, Bullet
};

/* bound to change by design, placeholders */
public class EnemyData : MonoBehaviour
{
    public EnemyType type;
    [SerializeField] bool isBoss;
    [SerializeField] int score = 100;
    [SerializeField] int rarity = 1;

    [Header("Run currency modifiers")]
    public int runCurrencyOnDeath;
    public float runCurrencyDropChance = 0.5f;

    [Header("Permanent currency modifiers")]
    public int permanentCurrencyOnDeath;
    public float permanentCurrencyDropChance = 0.15f;

    [Header("Health drop modifiers")]
    public GameObject healthDropPrefab;
    public float healthDropChance = 0.08f;


    void Awake()
    {
        GetComponent<HealthSystem>().events.OnDeath.AddListener(OnDeath);
    }

    void OnDeath()
    {
        // Roll for currency drop chances
        float roll = Random.Range(0.0f, 1.0f);
        Debug.Log("Death currency roll: " + roll);
        if (roll <= runCurrencyDropChance)
        {
            EventBus.Publish(new CurrencyEmissionEvent(transform.position, CurrencyType.Run, runCurrencyOnDeath));
        }
        roll = Random.Range(0.0f, 1.0f);
        if (roll <= permanentCurrencyDropChance)
        {
            EventBus.Publish(new CurrencyEmissionEvent(transform.position, CurrencyType.Permanent, permanentCurrencyOnDeath));
        }

        roll = Random.Range(0.0f, 1.0f);
        if (roll <= healthDropChance)
        {
            Vector3 location = transform.position + new Vector3(2, 0, 0);
            Instantiate(healthDropPrefab, location, Quaternion.identity);
        }
    }
}