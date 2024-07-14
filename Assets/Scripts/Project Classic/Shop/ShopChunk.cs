using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopChunk : MonoBehaviour
{
    public List<ShopTrigger> shopTriggers;

    bool triggeredEnterMessage = false;
    bool triggeredExitMessage = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!triggeredEnterMessage)
            {
                EventBus.Publish(new EnterShopEvent());
                triggeredEnterMessage = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!triggeredExitMessage)
            {
                EventBus.Publish(new ExitShopEvent());
                triggeredExitMessage = true;
            }
        }
    }
}
