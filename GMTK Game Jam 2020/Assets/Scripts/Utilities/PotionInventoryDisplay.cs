using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionInventoryDisplay : ItemInventoryDisplay
{
    public TextMeshProUGUI statBox;
    IPotion potion;

    public override void SetItem(IItem itemToDisplay, int amount)
    {

        if (!(itemToDisplay is IPotion)) return;
        base.SetItem(itemToDisplay, amount);
        potion = (IPotion) itemToDisplay;
        statBox.text = potion.stat + ": +" + potion.statValue;
    }

    protected override void StockItem()
    {
        GameManager.instance.StockPotion(potion);
    }
}
