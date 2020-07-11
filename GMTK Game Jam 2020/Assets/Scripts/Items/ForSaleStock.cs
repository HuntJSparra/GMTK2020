using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ForSaleStock : MonoBehaviour
{

    public ScrollRect stockRect;
    public int maxStock;
    public int maxStockPurchases;
    public GameObject equipmentDisplayPrefab;
    public float displayOffset = 10;
    public Button purchaseButton;

    public BoolEvent ReadyToSell;

    List<ItemStockDisplay> stockDisplays = new List<ItemStockDisplay>();
    List<EquipmentStockDisplay> equipmentStocks = new List<EquipmentStockDisplay>();
    public bool purchaseMode = false;
    int totalCost;
    TextMeshProUGUI title;
    TextMeshProUGUI purchaseButtonTitle;

    // Start is called before the first frame update
    void Start()
    {
        if (equipmentDisplayPrefab.GetComponent<EquipmentStockDisplay>() == null)
        {
            Debug.LogError("Error! Equipment Display Prefab does not have an EquipmentStockDisplay!");
            Application.Quit();
        }
        title = stockRect.GetComponentInChildren<TextMeshProUGUI>();
        purchaseButtonTitle = purchaseButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public bool AddEquipment(IEquipment item)
    {
        if ((!purchaseMode && stockDisplays.Count >= maxStock) || (purchaseMode && stockDisplays.Count >= maxStockPurchases))
            return false;
        EquipmentStockDisplay equipmentStock = Instantiate(equipmentDisplayPrefab, stockRect.content).GetComponent<EquipmentStockDisplay>();
        equipmentStock.SetItem(item);
        stockDisplays.Add(equipmentStock);
        equipmentStocks.Add(equipmentStock);
        if (purchaseMode) UpdateCost(item.GetBasePrice());
        ResizeStockRect();
        if (stockDisplays.Count == maxStock) ReadyToSell.Invoke(true);
        return true;
    }

    public bool RemoveItem(ItemStockDisplay item)
    {
        if (item is EquipmentStockDisplay) return RemoveEquipment((EquipmentStockDisplay) item);
        return false;
    }

    public bool RemoveEquipment(EquipmentStockDisplay item)
    {
        if (!equipmentStocks.Contains(item))
            return false;
        if (stockDisplays.Count == maxStock) ReadyToSell.Invoke(false);
        if (purchaseMode)
        {
            UpdateCost(-(item.GetItem().price));
        }
        equipmentStocks.Remove(item);
        stockDisplays.Remove(item);
        Destroy(item.gameObject);
        ResizeStockRect();
        return true;
    }

    void ResizeStockRect()
    {
        Vector2 prefabSize = equipmentDisplayPrefab.GetComponent<RectTransform>().sizeDelta;
        float contentHeight = (stockDisplays.Count * prefabSize.y) + ((stockDisplays.Count + 2) * displayOffset);
        stockRect.content.sizeDelta = new Vector2(stockRect.content.sizeDelta.x, contentHeight);
    }

    public void EnableStockButtons() { StockButtonsEnabler(true); }
    public void DisableStockButtons() { StockButtonsEnabler(false); }

    void StockButtonsEnabler(bool enableBtn)
    {
        foreach (ItemStockDisplay display in stockDisplays)
        {
            display.button.enabled = enableBtn;
        }
    }

    public void Clear()
    {
        while (stockDisplays.Count > 0)
        {
            RemoveItem(stockDisplays[0]);
        }
    }

    public void SwitchModes()
    {
        Clear();
        if(purchaseMode)
        {
            title.text = "For Sale";
            purchaseButton.gameObject.SetActive(false);
        }
        else
        {
            title.text = "Purchasing: ";
            purchaseButton.gameObject.SetActive(true);
        }
        purchaseMode = !purchaseMode;
    }

    public int GetTotalCost()
    {
        if (!purchaseMode) return -1;
        else return totalCost;
    }

    public void UpdateCost(int cost)
    {
        if (!purchaseMode) return;
        totalCost += cost;
        purchaseButtonTitle.text = "Purchase? Cost: " + totalCost;
        if (totalCost > GameManager.instance.wallet.money) purchaseButton.enabled = false;
        else purchaseButton.enabled = true;
    }

    public void MakePurchase()
    {
        if (totalCost > GameManager.instance.wallet.money) return;
        foreach (ItemStockDisplay item in stockDisplays)
            GameManager.instance.inventory.AddItem(item.GetItem());
        GameManager.instance.wallet.LoseMoney(totalCost);
        SwitchModes();
        totalCost = 0;
    }

    public List<IItem> GetAvaliableItems()
    {
        List<IItem> items = new List<IItem>();
        foreach (ItemStockDisplay item in stockDisplays) items.Add(item.GetItem());
        return items;
    }

}
