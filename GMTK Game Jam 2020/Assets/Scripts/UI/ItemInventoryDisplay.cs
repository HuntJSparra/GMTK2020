using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ItemInventoryDisplay : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI itemNameBox;
    public TextMeshProUGUI amountBox;
    public TextMeshProUGUI moneyBox;
    public Button minusButton;
    public Button plusButton;
    IItem item;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        minusButton.onClick.AddListener(SubtractPrice);
        plusButton.onClick.AddListener(AddPrice);
    }

    public void SetItem(IItem itemToDisplay, int amount)
    {
        item = itemToDisplay;
        icon.sprite = item.icon;
        itemNameBox.text = item.itemName;
        amountBox.text = "x "+amount.ToString();
        moneyBox.text = item.price.ToString();
    }

    public void OnClick()
    {
    }

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
