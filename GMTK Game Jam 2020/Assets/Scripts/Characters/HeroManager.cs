using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HeroManager : StatManager
{

    Image sprite;
    IItem chosenItem = null;

    private void Awake()
    {
        sprite = GetComponent<Image>();
    }

    public bool ChooseItem()
    {
        List<IItem> items = GameManager.instance.stock.GetAvaliableItems();
        if (items.Count == 0) { chosenItem = null; return false; }
        
        chosenItem = items[Random.Range(0, items.Count)];
        return true;
    }
    
    public IItem GetChosenItem() { return chosenItem; }
    public bool CanAffordItem()
    {
        if (chosenItem == null) return false;
        if (money < chosenItem.price) return false;
        return true;
    }

    public void BuyItem()
    {
        if (!CanAffordItem()) chosenItem.price = money;
        GameManager.instance.SellItem(chosenItem);
        money -= chosenItem.price;
        UseItem(chosenItem);
        chosenItem = null;
    }

    public string RequestString()
    {
        if (!CanAffordItem()) return string.Format("Hero: I would like to buy the {0}, but it's too expensive, nya. Can I buy it for {1}?", chosenItem.itemName, money);
        else return string.Format("Hero: I would like to buy the {0}, nya.", chosenItem.itemName);
    }

    public void UseItem(IItem item)
    {

    }

}
