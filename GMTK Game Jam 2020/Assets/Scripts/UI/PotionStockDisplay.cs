using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class PotionStockDisplay : ItemStockDisplay
{
    public TextMeshProUGUI statBox;
    IPotion potion;

    public void SetItem(IPotion itemToDisplay)
    {
        potion = itemToDisplay;
        base.SetItem((IItem)itemToDisplay);
        statBox.text = potion.stat + ": +" + potion.statValue;
    }

    public override void Empty()
    {
        base.Empty();
        potion = null;
        statBox.text = "";
    }
}
