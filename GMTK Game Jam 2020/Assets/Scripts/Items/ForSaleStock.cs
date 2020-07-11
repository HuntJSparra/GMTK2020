using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForSaleStock : MonoBehaviour
{

    public ScrollRect stockRect;
    public int maxStock;
    public GameObject equipmentDisplayPrefab;
    public float displayOffset = 10;

    public BoolEvent ReadyToSell;

    List<ItemStockDisplay> stockDisplays = new List<ItemStockDisplay>();
    List<EquipmentStockDisplay> equipmentStocks = new List<EquipmentStockDisplay>();

    // Start is called before the first frame update
    void Start()
    {
        if (equipmentDisplayPrefab.GetComponent<EquipmentStockDisplay>() == null)
        {
            Debug.LogError("Error! Equipment Display Prefab does not have an EquipmentStockDisplay!");
            Application.Quit();
        } 
    }

    public bool AddEquipment(IEquipment item)
    {
        if (stockDisplays.Count >= maxStock) return false;
        EquipmentStockDisplay equipmentStock = Instantiate(equipmentDisplayPrefab, stockRect.content).GetComponent<EquipmentStockDisplay>();
        equipmentStock.SetItem(item);
        stockDisplays.Add(equipmentStock);
        equipmentStocks.Add(equipmentStock);
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

}
