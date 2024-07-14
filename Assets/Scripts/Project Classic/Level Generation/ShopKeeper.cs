using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ShopKeeper : MonoBehaviour
{
    public GameObject shopScreen;

    Subscription<ShopSelectEvent> shop_select_sub;

    [Header("Movement")]
    public Transform ShopPosLeft;
    public Transform ShopPosRight;
    public float speed;

    void Start()
    {
        if(shopScreen != null) shopScreen.SetActive(false);
        EventBus.Subscribe<ShopSelectEvent>(_OnShopItemSelected);
    }

    public void PlayerInteract()
    {
        if (shopScreen != null && !shopScreen.activeInHierarchy)
        {
            shopScreen.SetActive(true);
        }
    }

    public void PlayerLeave()
    {
        // Only close the shop if it's active
        if (shopScreen != null && shopScreen.activeInHierarchy)
        {
            shopScreen.SetActive(false);
        }
    }

    void _OnShopItemSelected(ShopSelectEvent e)
    {
        //(optional) shopkeeper will pace between the items depending on what the player is looking at
        //StartCoroutine(Move(e.item_selected));
    }

    IEnumerator Move(int shop_pos)
    {
        Vector2 target = transform.position;
        switch (shop_pos)
        {
            case 0:
                target = ShopPosLeft.position; break;
            case 1:
                target = transform.position; break;
            case 2:
                target = ShopPosRight.position; break;
            default:
                break;
        }

        while(transform.position.x != target.x)
        {
            transform.position = new Vector2(Vector2.Lerp(transform.position, target, speed).x, transform.position.y);
            yield return null;
        }
    }
}
