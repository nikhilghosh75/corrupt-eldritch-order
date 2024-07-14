using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using WSoft.Audio;

public class ClassSelectMenu : MonoBehaviour
{
    public AudioEvent classLocked;
    public AudioEvent classSelected;

    public GameObject playerPrefab;
    public List<GameObject> classCards = new List<GameObject>();
    public string selectedClass = "Default";

    public MenuManager menuManager;

    public StatDisplayBar speedDisplay, damageDisplay, healthDisplay, manaChargeDisplay, rollDistanceDisplay, fireRateDisplay;

    private float maxSpeed, maxDamage, maxHealth, maxManaCharge, maxRollDistance, maxFireRate;

    private PlayerClassBase defaultClass;
    private PlayerClassNinja ninjaClass;
    private PlayerClassWizard wizardClass;
    private PlayerClassTank tankClass;

    private bool carouselAnimating = false;

    public GameObject button;

    private void Start()
    {
        defaultClass = playerPrefab.GetComponent<PlayerClassBase>();
        ninjaClass = playerPrefab.GetComponent<PlayerClassNinja>();
        wizardClass = playerPrefab.GetComponent<PlayerClassWizard>();
        tankClass = playerPrefab.GetComponent<PlayerClassTank>();

        SetMaxStats();

        UpdateCurrentlySelectedCard();
    }

    public void RotateCardsRight()
    {
        if (!carouselAnimating)
            StartCoroutine(PlayCardAnimations(false));
    }

    public void RotateCardsLeft()
    {
        if (!carouselAnimating)
            StartCoroutine(PlayCardAnimations(true));
    }

    public IEnumerator PlayCardAnimations(bool isReversed = false)
    {
        carouselAnimating = true;

        ClassCardAnimationController animationController =  classCards[0].GetComponent<ClassCardAnimationController>();
        animationController.cardIndex = 0;
        animationController.MoveAndAnimate(isReversed);

        animationController = classCards[1].GetComponent<ClassCardAnimationController>();
        animationController.cardIndex = 1;
        animationController.MoveAndAnimate(isReversed);

        animationController = classCards[2].GetComponent<ClassCardAnimationController>();
        animationController.cardIndex = 2;
        animationController.MoveAndAnimate(isReversed);

        animationController = classCards[3].GetComponent<ClassCardAnimationController>();
        animationController.cardIndex = 3;
        animationController.MoveAndAnimate(isReversed);

        if (isReversed)
            RotateLeft(classCards);
        else
            RotateRight(classCards);

        yield return new WaitForSeconds(animationController.animationDuration);
        carouselAnimating = false;

        UpdateCurrentlySelectedCard();
    }

    public void RotateRight(List<GameObject> cards)
    {
        if (cards.Count > 0)
        {
            GameObject first = cards[0];
            cards.RemoveAt(0);
            cards.Add(first);
        }
    }

    public void RotateLeft(List<GameObject> cards)
    {
        if (cards.Count > 0)
        {
            GameObject last = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);
            cards.Insert(0, last);
        }
    }

    private void UpdateCurrentlySelectedCard()
    {
        selectedClass = classCards[0].GetComponent<ClassCardAnimationController>().className;

        if (selectedClass == "Default")
        {
            UpdateClassStatDisplay(defaultClass.speed / maxSpeed,
                                           defaultClass.damageMultiplier / maxDamage,
                                           defaultClass.maxHealth / maxHealth,
                                           defaultClass.manaChargeRate / maxManaCharge,
                                           defaultClass.dodgeLength / maxRollDistance,
                                           defaultClass.fireRate / maxFireRate);
            return;
        }

        if (selectedClass == "Ninja")
        {
            UpdateClassStatDisplay(ninjaClass.speedMod / maxSpeed,
                               ninjaClass.damageMod / maxDamage,
                               ninjaClass.healthMod / maxHealth,
                               ninjaClass.manaChargeMod / maxManaCharge,
                               ninjaClass.dodgeLengthMod / maxRollDistance,
                               ninjaClass.fireRateMod / maxFireRate);
            return;
        }

        if (selectedClass == "Wizard")
        {
            UpdateClassStatDisplay(wizardClass.speedMod / maxSpeed,
                               wizardClass.damageMod / maxDamage,
                               wizardClass.healthMod / maxHealth,
                               wizardClass.manaChargeMod / maxManaCharge,
                               wizardClass.dodgeLengthMod / maxRollDistance,
                               wizardClass.fireRateMod / maxFireRate);
            return;
        }

        if (selectedClass == "Tank")
        {
            UpdateClassStatDisplay(tankClass.speedMod / maxSpeed,
                               tankClass.damageMod / maxDamage,
                               tankClass.healthMod / maxHealth,
                               tankClass.manaChargeMod / maxManaCharge,
                               tankClass.dodgeLengthMod / maxRollDistance,
                               tankClass.fireRateMod / maxFireRate);
            return;
        }
    }

    private void UpdateClassStatDisplay(float speedPercentage, float damagePercentage, float healthPercentage, float manaChargePercentage, float rollDistancePercentage, float fireRatePercentage)
    {
        speedDisplay.UpdateStatBar(speedPercentage);
        damageDisplay.UpdateStatBar(damagePercentage);
        healthDisplay.UpdateStatBar(healthPercentage);
        manaChargeDisplay.UpdateStatBar(manaChargePercentage);
        rollDistanceDisplay.UpdateStatBar(rollDistancePercentage);
        fireRateDisplay.UpdateStatBar(fireRatePercentage);
    }

    private void SetMaxStats()
    {
        // Default max values
        maxSpeed = defaultClass.speed;
        maxDamage = defaultClass.damageMultiplier;
        maxHealth = defaultClass.maxHealth;
        maxManaCharge = defaultClass.manaChargeRate;
        maxRollDistance = defaultClass.dodgeLength;
        maxFireRate = defaultClass.fireRate;

        // Ninja Class
        maxSpeed = maxSpeed < ninjaClass.speedMod ? ninjaClass.speedMod : maxSpeed;
        maxDamage = maxDamage < ninjaClass.damageMod? ninjaClass.damageMod : maxDamage;
        maxHealth = maxHealth < ninjaClass.healthMod ? ninjaClass.healthMod : maxHealth;
        maxManaCharge = maxManaCharge < ninjaClass.manaChargeMod ? ninjaClass.manaChargeMod : maxManaCharge;
        maxRollDistance = maxRollDistance < ninjaClass.dodgeLengthMod ? ninjaClass.dodgeLengthMod : maxRollDistance;
        maxFireRate = maxFireRate < ninjaClass.fireRateMod ? ninjaClass.fireRateMod : maxFireRate;

        // Wizard Class
        maxSpeed = maxSpeed < wizardClass.speedMod ? wizardClass.speedMod : maxSpeed;
        maxDamage = maxDamage < wizardClass.damageMod ? wizardClass.damageMod : maxDamage;
        maxHealth = maxHealth < wizardClass.healthMod ? wizardClass.healthMod : maxHealth;
        maxManaCharge = maxManaCharge < wizardClass.manaChargeMod ? wizardClass.manaChargeMod : maxManaCharge;
        maxRollDistance = maxRollDistance < wizardClass.dodgeLengthMod ? wizardClass.dodgeLengthMod : maxRollDistance;
        maxFireRate = maxFireRate < wizardClass.fireRateMod ? wizardClass.fireRateMod : maxFireRate;

        // Tank Class
        maxSpeed = maxSpeed < tankClass.speedMod ? tankClass.speedMod : maxSpeed;
        maxDamage = maxDamage < tankClass.damageMod ? tankClass.damageMod : maxDamage;
        maxHealth = maxHealth < tankClass.healthMod ? tankClass.healthMod : maxHealth;
        maxManaCharge = maxManaCharge < tankClass.manaChargeMod ? tankClass.manaChargeMod : maxManaCharge;
        maxRollDistance = maxRollDistance < tankClass.dodgeLengthMod ? tankClass.dodgeLengthMod : maxRollDistance;
        maxFireRate = maxFireRate < tankClass.fireRateMod ? tankClass.fireRateMod : maxFireRate;
    }

    public void SelectClass()
    {
        // Don't let the user select any class that hasn't been unlocked yet
        if (!RunManager.Instance.unlockedClasses.Contains(selectedClass))
        {
            classLocked.PlayAudio(button);
            return;
        }

        classSelected.PlayAudio(button);
        RunManager.Instance.SetSelectedClassName(selectedClass);
        menuManager.TransitionToCanvas("Weapon Select Menu Canvas");
    }
}
