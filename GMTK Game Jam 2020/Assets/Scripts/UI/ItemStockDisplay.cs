using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ItemStockDisplay : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI itemNameBox;
    public TextMeshProUGUI moneyBox;
    IItem item;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void SetItem(IItem itemToDisplay)
    {
        item = itemToDisplay;
        icon.sprite = item.icon;
        itemNameBox.text = item.itemName;
        moneyBox.text = item.price.ToString();
    }

    public void OnClick()
    {
    }


    

}
