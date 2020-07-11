using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class GameManager : MonoBehaviour
{

    public enum GameStates
    {
        Purchasing,
        Restocking,
        Start,
        Stocking, 
        Selling,
        CompletePurchase
    }

    public static GameManager instance;
    public GameStates state = GameStates.Start;
    public Inventory inventory;
    public ForSaleStock stock;
    public Wallet wallet;
    public GameObject IItemPrefab;

    public UnityEvent OnPurchasingEnter;
    public UnityEvent OnCompletePurchase;
    public UnityEvent OnSellingEnter;
    public UnityEvent OnStockingEnter;
    public UnityEvent OnRestockingEnter;

    Animator stateMachine;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        stock.ReadyToSell.AddListener(FullyStocked);
    }

    private void Start()
    {
        stateMachine = GetComponent<Animator>();
        /*for (int i=0; i<20; i++)
        {
            GameObject IItemObject = Instantiate(IItemPrefab);
            IItem item = IItemObject.GetComponent<IItem>();
            item.itemName = "Item " + i;
            inventory.AddItem(item);
        }*/
        inventory.GenerateInventoryGUI();

    }

    bool ItemInStock(IItem item)
    {
        if (inventory.GetAmount(item) <= 0) return false;
        return true;
    }

    public void StockEquipment(IEquipment item)
    {
        if (state != GameStates.Purchasing)
        {
            if (!ItemInStock(item)) return;
            inventory.DecrementAmount(item);
        }
        stock.AddEquipment(item);
    }

    public void ChangeState(GameStates newState)
    {
        state = newState;
        switch (state)
        {
            case (GameStates.Purchasing):
                OnPurchasingEnter.Invoke();
                break;
            case (GameStates.CompletePurchase):
                OnCompletePurchase.Invoke();
                break;
            case(GameStates.Stocking):
                OnStockingEnter.Invoke();
                break;
            case (GameStates.Selling):
                OnSellingEnter.Invoke();
                break;
            case (GameStates.Restocking):
                OnRestockingEnter.Invoke();
                break;
        }
    }

    public void Purchasing(bool purchasing)
    {
        stateMachine.SetBool("Purchasing", purchasing);
    }

    public void Selling(bool selling)
    {
        stateMachine.SetBool("Selling", selling);
    }
    
    public void FullyStocked(bool stocked)
    {
        stateMachine.SetBool("FullyStocked", stocked);
    }


}
