using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IItem : MonoBehaviour
{
    public string itemName;
    public Sprite icon;
    public string description;
    public int price;

    public override bool Equals(object other)
    {
        if (!(other is IItem)) return false;
        IItem otherItem = (IItem) other;
        return itemName.Equals(otherItem.itemName);
    }

    public override int GetHashCode()
    {
        return itemName.GetHashCode();
    }

}
