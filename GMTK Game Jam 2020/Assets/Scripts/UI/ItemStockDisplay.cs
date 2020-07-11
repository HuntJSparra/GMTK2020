using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public abstract class ItemStockDisplay : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI itemNameBox;
    public TextMeshProUGUI moneyBox;
    public Button button;
    IItem item;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void SetItem(IItem itemToDisplay)
    {
        item = itemToDisplay;
        icon.sprite = item.icon;
        itemNameBox.text = item.itemName;
        moneyBox.text = item.price.ToString();
    }


    public virtual void Empty()
    {
        item = null;
        itemNameBox.text = "";
        moneyBox.text = "";
    }

    public void OnClick()
    {
        switch (GameManager.instance.state)
        {
            case (GameManager.GameStates.Stocking):
                Restock();
                break;
            case (GameManager.GameStates.Restocking):
                Restock();
                break;
            case (GameManager.GameStates.Selling):
                Sell();
                break;
        }
    }

    protected void Restock()
    {
        GameManager.instance.inventory.AddItem(item);
        GameManager.instance.stock.RemoveItem(this);
    }

    protected void Sell()
    {
        GameManager.instance.wallet.EarnMoney(item.price);
        GameManager.instance.stock.RemoveItem(this);
    }
    


    

}
