using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WSoft.Audio;

public class GenerateManaEvent
{
    public int manaGenerated;

    public GenerateManaEvent(int _manaGenerated)
    {
        manaGenerated = _manaGenerated;
    }
}

/// <summary>
/// A class that manages the player's mana.
/// </summary>
public class PlayerMana : MonoBehaviour
{
    [SerializeField] public float maxMana = 100.0f;

    [SerializeField] private float timeSinceLastManaGenerationEvent = 0;
    
    [Tooltip("The amount of seconds between passive mana generate events.")]
    [SerializeField] private float manaGenerateTick = 0.5f;

    [Tooltip("The amount of mana generated on each tic.")]
    [SerializeField] private float manaGenerateAmount = 0.5f;

    [Tooltip("How long passive generation stops for when the player gets hit.")]
    [SerializeField] private float passiveGenerationPauseTime = 2f;

    private float manaPassiveTimer = 0;
    
    public float currentMana = 0.0f;
    public float manaGenerationMultiplier = 1f;
    bool passivelyGenerating = true;
    private Coroutine manaDrainCoroutine = null; // Reference to the active mana drain coroutine

    public AudioEvent manaCharge;
    private bool chargePlayed = false;

    bool canGainMana = true;

    private void Start()
    {
        EventBus.Subscribe<GenerateManaEvent>(OnGenerateManaEvent);
        EventBus.Subscribe<UpdateHealthEvent>(OnUpdateHealthEvent);
    }

    private void Update()
    {
        timeSinceLastManaGenerationEvent += Time.deltaTime;
        manaPassiveTimer += Time.deltaTime;

        if ((manaPassiveTimer >= manaGenerateTick) && passivelyGenerating && canGainMana)
        {
            manaPassiveTimer = 0;
            currentMana += manaGenerateAmount * manaGenerationMultiplier;
        }
    }

    public void StartManaDrain(float drainRate)
    {
        StartCoroutine(PerformManaDrain(drainRate));
    }

    IEnumerator PerformManaDrain(float drainRate)
    {
        canGainMana = false;
        float timeToDrain = currentMana / drainRate;
        while (currentMana >= -1f)
        {
            currentMana -= drainRate * Time.deltaTime;
            yield return null;
        }
        canGainMana = true;
        currentMana = -1f;
        ReenablePassiveGeneration();
    }

    void OnUpdateHealthEvent(UpdateHealthEvent e)
    {
        // When the player takes damage, pause the passive mana generation
        if (e.healthDelta < 0)
        {
            passivelyGenerating = false;
            CancelInvoke(nameof(ReenablePassiveGeneration));
            Invoke(nameof(ReenablePassiveGeneration), passiveGenerationPauseTime);
        }
    }

    void ReenablePassiveGeneration()
    {
        passivelyGenerating = true;
        manaPassiveTimer = 0;
    }

    private void OnGenerateManaEvent(GenerateManaEvent e)
    {
        if (!canGainMana)
            return;

        timeSinceLastManaGenerationEvent = 0;
        currentMana += e.manaGenerated * manaGenerationMultiplier;

        if (currentMana > maxMana)
        {
            currentMana = maxMana;
            if (chargePlayed == false)
            {
                manaCharge.PlayAudio(gameObject);
                chargePlayed = true;
            }
        }
    }

    public float GetCurrentMana()
    {
        return currentMana;
    }

    /// <summary>
    /// Attempts to consume a specified amount of mana. Returns true if successful, false otherwise.
    /// </summary>
    /// <param name="amount">The amount of mana to consume.</param>
    /// <returns>Whether the mana was successfully consumed.</returns>
    public bool ConsumeMana(float amount)
    {
        chargePlayed = false; 

        if (currentMana >= amount)
        {
            currentMana -= amount;
            return true;
        }
        else
        {
            // Not enough mana
            return false;
        }
    }
}
