using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public GameObject equipmentDisplayPrefab;
    public float displayOffset = 10;
    public ScrollRect scrollRect;
    List<IItem> items = new List<IItem>();
    Dictionary<IItem, int> amounts = new Dictionary<IItem, int>();
    Dictionary<IItem, ItemInventoryDisplay> displays = new Dictionary<IItem, ItemInventoryDisplay>(); 

    void Start()
    {
        if (equipmentDisplayPrefab.GetComponent<ItemInventoryDisplay>() == null)
        {
            Debug.LogError("Error! The Inventory Display Prefab must have an ItemInventoryDisplay!");
            Application.Quit();
        }
    }

    public void AddItem(IItem item, int amount = 1)
    {
        if (items.Contains(item))
        {
            IncrementAmount(item, amount);
            return;
        }
        items.Add(item);
        amounts.Add(item, amount);
        if (item is IEquipment)
        {
            IEquipment equip = (IEquipment)item;
            AddEquipment(equip);
        }
    }

    void AddEquipment(IEquipment item)
    {
        GameObject newDisplayObject = Instantiate(equipmentDisplayPrefab, scrollRect.content);
        newDisplayObject.name = equipmentDisplayPrefab.name + " " + item.itemName;
        EquipmentInventoryDisplay newDisplay = newDisplayObject.GetComponent<EquipmentInventoryDisplay>();
        newDisplay.SetItem(item, amounts[item]);
        displays.Add(item, newDisplay);
    }

    public void IncrementAmount(IItem item, int amount=1)
    {
        amounts[item] = amounts[item] + amount;
        if (amounts[item] > 999) amounts[item] = 999;
        displays[item].SetAmount(amounts[item]);
    }

    public void DecrementAmount(IItem item, int amount = 1)
    {
        amounts[item] = amounts[item] - amount;
        if (amounts[item] < 0) amounts[item] = 0;
        displays[item].SetAmount(amounts[item]);
    }

    public int GetAmount(IItem item)
    {
        if (!amounts.ContainsKey(item)) return -1;
        return amounts[item];
    }

    public void GenerateInventoryGUI()
    {
        Vector2 prefabSize = equipmentDisplayPrefab.GetComponent<RectTransform>().sizeDelta;
        float contentHeight = (items.Count * prefabSize.y) + ((items.Count + 2) * displayOffset);
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, contentHeight);
    }
}
