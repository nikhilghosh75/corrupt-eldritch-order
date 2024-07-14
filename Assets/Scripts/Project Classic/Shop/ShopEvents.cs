using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterShopEvent
{

}

public class ExitShopEvent
{

}

public class ShopSelectEvent
{
    public int item_selected = 1;

    public ShopSelectEvent(int _item_selected)
    {
        item_selected = _item_selected;
    }
}

public class ShopPurchaseEvent
{

}