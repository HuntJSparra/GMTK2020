using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Inventory inventory;
    public GameObject IItemPrefab;

    private void Start()
    {
        for (int i=0; i<20; i++)
        {
            GameObject IItemObject = Instantiate(IItemPrefab);
            IItem item = IItemObject.GetComponent<IItem>();
            item.itemName = "Item " + i;
            inventory.AddItem(item);
            inventory.GenerateInventoryGUI();
        }

    }


}
