using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public List<SOPowerup> possiblePowerups;

    public string shopTitle;

    int indexChosen;
    private SOWeaponPowerup weaponPowerup;

    public bool weaponShop;
    public enum Position { Left, Center, Right };
    public Position ItemPosition;

    void Start()
    {
        
    }

    public void SetIndex(int seed)
    {
        System.Random random = new System.Random(seed);
        indexChosen = random.Next(possiblePowerups.Count);

        // Determine rarity
        if (weaponShop)
        {
            weaponPowerup = (SOWeaponPowerup)possiblePowerups[indexChosen];
            weaponPowerup.GenerateRarity(random);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            string description = possiblePowerups[indexChosen].displayName;

            if(weaponShop)
            {
                description += " - " + weaponPowerup.rarity;

                foreach(WeaponMod mod in weaponPowerup.weaponMods)
                {
                    description += "\n" + mod.displayName;
                }
            }

            ShopDisplay.instance.gameObject.SetActive(true);
            ShopDisplay.instance.ShowDisplay(shopTitle, description);

            EventBus.Publish(new ShopSelectEvent((int)ItemPosition));
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
            ShopDisplay.instance.gameObject.SetActive(false);
    }

    public void Purchase(PlayerPowerupManager powerupManager)
    {
        SOPowerup powerupChosen = possiblePowerups[indexChosen];

        if (RunManager.Instance.runCurrency >= powerupChosen.cost)
        {
            powerupChosen.ApplyPowerup(powerupManager);
            EventBus.Publish(new ItemPurchasedEvent(powerupChosen.cost, powerupChosen));
            EventBus.Publish(new ShopPurchaseEvent());
            Destroy(gameObject);
        }
    }
}