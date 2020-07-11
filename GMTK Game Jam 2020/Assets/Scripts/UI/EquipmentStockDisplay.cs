using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class EquipmentStockDisplay : ItemStockDisplay
{
    public TextMeshProUGUI atkBox;
    public TextMeshProUGUI defBox;
    public Color positiveColor;
    public Color negativeColor;
    IEquipment equipment;

    public void SetItem(IEquipment itemToDisplay)
    {
        equipment = itemToDisplay;
        base.SetItem((IItem)itemToDisplay);
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

    public override void Empty()
    {
        base.Empty();
        equipment = null;
        atkBox.text = "";
        defBox.text = "";
    }



}
