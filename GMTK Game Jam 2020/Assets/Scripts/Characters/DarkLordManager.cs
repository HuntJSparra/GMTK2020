using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarkLordManager : MonoBehaviour
{
    public Image sprite;
    public int money = 100;
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
        if (!CanAffordItem()) return string.Format("Dark Lord: I like that {0}. You WILL give it to me for {1}!", chosenItem.itemName, money);
        else return string.Format("Dark Lord: You're going to give me {0}!", chosenItem.itemName);
    }

    public string LeaveString()
    {
        if (chosenItem != null)
        {
            chosenItem = null;
            return "Dark Lord: You will regret this, mark my words...";
        }
        return "Dark Lord: Your services are appreciated, weakling.";
    }

    public void UseItem(IItem item)
    {
        if (item is IEquipment)
        {
            IEquipment equipment = (IEquipment)item;
            GameManager.instance.IncreaseActiveQuestDefense(equipment.itemOffense);
            GameManager.instance.IncreaseActiveQuestAttack(equipment.itemDefense);
        }
    }
}
