using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public GameObject equipmentDisplayPrefab;
    public GameObject potionDisplayPrefab;
    public float displayOffset = 10;
    public ScrollRect scrollRect;

    TextMeshProUGUI scrollTitle;
    bool catalogueMode = false;

    public List<IItem> level0Items = new List<IItem>();
    int level = -1;

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
        scrollTitle = scrollRect.GetComponentInChildren<TextMeshProUGUI>();
        //UnlockItems();
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
        else if (item is IPotion)
        {
            IPotion potion = (IPotion)item;
            AddPotion(potion);
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

    void AddPotion(IPotion item)
    {
        GameObject newDisplayObject = Instantiate(potionDisplayPrefab, scrollRect.content);
        newDisplayObject.name = potionDisplayPrefab.name + " " + item.itemName;
        PotionInventoryDisplay newDisplay = newDisplayObject.GetComponent<PotionInventoryDisplay>();
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

    public void SwitchModes()
    {
        foreach (ItemInventoryDisplay display in displays.Values)
        {
            display.SwitchModes();
        }

        if (catalogueMode)
        {
            scrollTitle.text = "Inventory";
            catalogueMode = false;
        }
        else
        {
            scrollTitle.text = "Catalogue";
            catalogueMode = true;
        }
    }

    public void UnlockItems()
    {
        level++;
        if (level == 0) AddToInventory(level0Items);
    }

    void AddToInventory(List<IItem> items)
    {
        foreach (IItem item in items)
            AddItem(item, 0);
        GenerateInventoryGUI();
    }

    public int GetTotalInventory()
    {
        int sum = 0;
        foreach (int amount in amounts.Values)
            sum += amount;
        return sum;
    }
   

}
