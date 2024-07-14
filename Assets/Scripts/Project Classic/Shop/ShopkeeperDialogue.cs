using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopkeeperDialogue : MonoBehaviour
{
    Subscription<EnterShopEvent> shop_enter_sub;
    Subscription<ExitShopEvent> shop_exit_sub;
    Subscription<ShopSelectEvent> shop_select_sub;
    Subscription<ShopPurchaseEvent> shop_purchase_sub;

    [Header("General")]
    public float DialogueDisplayTime = 3.0f;
    public float TimeBetweenLines = 15.0f;

    [Header("Event Specific Dialogue")]
    public string DialogueOnFirstEnter;
    public string DialogueOnSecondEnter;

    [Header("Random Dialogue Options")]
    public string[] OnEnterDialogueOptions;
    public string[] OnExitDialogueOptionsNoPurchase;
    public string[] OnExitDialogueOptionsWithPurchase;
    public string[] OnExitDialogueOptions;
    public string[] OnShopSelectDialogueOptions;
    public string[] OnShopPurchaseDialogueOptions;
    public string[] IdleDialogueOptions;

    public TextMeshProUGUI text;

    private bool made_purchase = false;

    private float time_since_last_dialogue = 0.0f;

    //TODO: this number has to stay between scene loads, if new levels are loaded
    private int num_visits = 0;

    // Start is called before the first frame update
    void Start()
    {
        shop_enter_sub = EventBus.Subscribe<EnterShopEvent>(_OnShopEntered);
        shop_exit_sub = EventBus.Subscribe<ExitShopEvent>(_OnShopExited);
        shop_select_sub = EventBus.Subscribe<ShopSelectEvent>(_OnShopItemSelected);
        shop_purchase_sub = EventBus.Subscribe<ShopPurchaseEvent>(_OnShopItemPurchased);
    }

    private void Update()
    {
        time_since_last_dialogue += Time.deltaTime;
        if(time_since_last_dialogue > TimeBetweenLines)
        {
            time_since_last_dialogue = 0.0f;
            int random_index = Random.Range(0, IdleDialogueOptions.Length);
            StartCoroutine(ShowDialogue(IdleDialogueOptions[random_index]));
        }
    }

    void _OnShopEntered(EnterShopEvent e)
    {
        if(num_visits == 0)
        {
            StartCoroutine(ShowDialogue(DialogueOnFirstEnter));
        }
        else if(num_visits == 1)
        {
            StartCoroutine(ShowDialogue(DialogueOnSecondEnter));
        }
        else
        {
            int random_index = Random.Range(0, OnEnterDialogueOptions.Length);
            StartCoroutine(ShowDialogue(OnEnterDialogueOptions[random_index]));
        }
        num_visits++;
    }

    void _OnShopExited(ExitShopEvent e)
    {
        if (made_purchase)
        {
            int random_index = Random.Range(0, OnExitDialogueOptionsWithPurchase.Length + OnExitDialogueOptions.Length);
            if(random_index >= OnExitDialogueOptions.Length)
            {
                random_index %= OnExitDialogueOptions.Length;
                StartCoroutine(ShowDialogue(OnExitDialogueOptionsWithPurchase[random_index]));
            }
            else
            {
                StartCoroutine(ShowDialogue(OnExitDialogueOptions[random_index]));
            }
        }
        else
        {
            int random_index = Random.Range(0, OnExitDialogueOptionsNoPurchase.Length + OnExitDialogueOptions.Length);
            if (random_index >= OnExitDialogueOptions.Length)
            {
                random_index %= OnExitDialogueOptions.Length;
                StartCoroutine(ShowDialogue(OnExitDialogueOptionsNoPurchase[random_index]));
            }
            else
            {
                StartCoroutine(ShowDialogue(OnExitDialogueOptions[random_index]));
            }
        }
        made_purchase = false;
    }

    void _OnShopItemSelected(ShopSelectEvent e)
    {
        int random_index = Random.Range(0, OnShopSelectDialogueOptions.Length);
        StartCoroutine(ShowDialogue(OnShopSelectDialogueOptions[random_index]));
    }

    void _OnShopItemPurchased(ShopPurchaseEvent e)
    {
        int random_index = Random.Range(0, OnShopPurchaseDialogueOptions.Length);
        StartCoroutine(ShowDialogue(OnShopPurchaseDialogueOptions[random_index]));
        made_purchase = true;
    }

    IEnumerator ShowDialogue(string dialogue)
    {
        text.text = dialogue;
        yield return new WaitForSeconds(DialogueDisplayTime);
        text.text = string.Empty;
        time_since_last_dialogue = 0.0f;
    }
}
