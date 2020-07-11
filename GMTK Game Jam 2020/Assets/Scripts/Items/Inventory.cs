using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public GameObject displayPrefab;
    public float displayOffset = 10;
    public ScrollRect scrollRect;
    List<IItem> items = new List<IItem>();
    Dictionary<IItem, int> amounts = new Dictionary<IItem, int>();
    Dictionary<IItem, ItemInventoryDisplay> displays = new Dictionary<IItem, ItemInventoryDisplay>(); 

    void Start()
    {
        if (displayPrefab.GetComponent<ItemInventoryDisplay>() == null)
        {
            Debug.LogError("Error! The Inventory Display Prefab must have an ItemInventoryDisplay!");
            Application.Quit();
        }
    }

    public void AddItem(IItem item, int amount=1)
    {
        if (items.Contains(item)) IncrementAmount(item, amount);
        items.Add(item);
        amounts.Add(item, amount);
    }

    public void IncrementAmount(IItem item, int amount=1)
    {
        amounts[item] = amounts[item] + amount;
        if (amounts[item] > 999) amounts[item] = 999;
    }

    public void DecrementAmount(IItem item, int amount = 1)
    {
        amounts[item] = amounts[item] - amount;
        if (amounts[item] < 0) amounts[item] = 0;
    }

    public void GenerateInventoryGUI()
    {
        Vector2 prefabSize = displayPrefab.GetComponent<RectTransform>().sizeDelta;
        float contentHeight = (items.Count * prefabSize.y) + ((items.Count + 2) * displayOffset);
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, contentHeight);

        foreach (IItem item in items)
        {
            if (!displays.ContainsKey(item))
            {
                GameObject newDisplayObject = Instantiate(displayPrefab, scrollRect.content);
                newDisplayObject.name = displayPrefab.name + " " + item.itemName;
                ItemInventoryDisplay newDisplay = newDisplayObject.GetComponent<ItemInventoryDisplay>();
                newDisplay.SetItem(item, amounts[item]);
                displays.Add(item, newDisplay);
            }
        }
    }
}
