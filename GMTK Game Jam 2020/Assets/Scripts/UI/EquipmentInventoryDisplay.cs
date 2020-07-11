using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentInventoryDisplay : ItemInventoryDisplay
{
    public TextMeshProUGUI atkBox;
    public TextMeshProUGUI defBox;
    public Color positiveColor = Color.red;
    public Color negativeColor = Color.blue;
    IEquipment equipment;

    public override void SetItem(IItem itemToDisplay, int amount)
    {

        if (!(itemToDisplay is IEquipment)) return;
        base.SetItem(itemToDisplay, amount);
        equipment = (IEquipment) itemToDisplay;
        setStatBox(equipment.itemOffense, atkBox);
        setStatBox(equipment.itemDefense, defBox);
    }

    void setStatBox(int stat, TextMeshProUGUI box)
    {
        if (stat > 0)
        {
            box.text = "+" + stat.ToString();
        }
        else if (stat < 0)
        {
            box.text = "-" + stat.ToString();
        }
        else
        {
            box.text = stat.ToString();
        }
    }

    protected override void StockItem()
    {
        GameManager.instance.StockEquipment(equipment);
    }

}
