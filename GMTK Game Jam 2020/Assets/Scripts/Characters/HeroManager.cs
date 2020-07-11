using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : StatManager
{


    public IItem ChooseItem()
    {
        List<IItem> items = GameManager.instance.stock.GetAvaliableItems();
        if (items.Count == 0) return null;
        
        IItem chosenItem = items[Random.Range(0, items.Count)];
        return chosenItem;
    }



}
