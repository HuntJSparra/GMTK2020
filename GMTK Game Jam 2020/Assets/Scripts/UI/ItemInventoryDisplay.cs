using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public abstract class ItemInventoryDisplay : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI itemNameBox;
    public TextMeshProUGUI amountBox;
    public TextMeshProUGUI moneyBox;
    public Button minusButton;
    public Button plusButton;
    public Button button;
    IItem item;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        minusButton.onClick.AddListener(SubtractPrice);
        plusButton.onClick.AddListener(AddPrice);
    }

    public virtual void SetItem(IItem itemToDisplay, int amount)
    {
        item = itemToDisplay;
        icon.sprite = item.icon;
        itemNameBox.text = item.itemName;
        moneyBox.text = item.price.ToString();
        SetAmount(amount);
    }

    public void SetAmount(int amount)
    {
        amountBox.text = "x " + amount.ToString();
        if (amount <= 0) button.enabled = false;
        else button.enabled = true;
    }

    public void OnClick()
    {
        StockItem();
    }

    protected abstract void StockItem();

    public void SubtractPrice()
    {
        item.price--;
        if (item.price < 0) item.price = 0;
        moneyBox.text = item.price.ToString();
    }

    public void AddPrice()
    {
        item.price++;
        if (item.price > 999) item.price = 999;
        moneyBox.text = item.price.ToString();
    }


}
