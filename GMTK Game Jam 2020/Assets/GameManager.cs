using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
    public HeroManager hero;
    public DarkLordManager darkLord;
    public TextMeshProUGUI textbox;
    public Button nextButton;
    public Button yesButton;
    public Button noButton;
    public QuestDisplay questDisplay;
    public Quest[] quests = new Quest[4];

    public UnityEvent OnPurchasingEnter;
    public UnityEvent OnCompletePurchase;
    public UnityEvent OnSellingEnter;
    public UnityEvent OnStockingEnter;
    public UnityEvent OnRestockingEnter;
    public UnityEvent HeroAppears;
    public UnityEvent DarkLordAppears;

    int numTransactions = -1;
    int activeQuest = -1;
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
        inventory.GenerateInventoryGUI();
        StartNextQuest();
    }

    bool ItemInStock(IItem item)
    {
        if (inventory.GetAmount(item) <= 0) return false;
        return true;
    }

    public void StockEquipment(IEquipment item)
    {
        bool stocked = stock.AddEquipment(item);
        if (stocked && state != GameStates.Purchasing)
        {
            if (!ItemInStock(item)) return;
            inventory.DecrementAmount(item);
        }
    }

    public bool SellItem(IItem item)
    {
        if (!stock.RemoveItem(item)) return false;
        wallet.EarnMoney(item.price);
        return true;
    }

    public void SetText(string text)
    {
        textbox.text = text;
        if (!text.Equals(string.Empty))
            nextButton.gameObject.SetActive(true);
    }

    public void GiveChoice(string yesText, string noText)
    {
        yesButton.GetComponentInChildren<TextMeshProUGUI>().text = yesText;
        noButton.GetComponentInChildren<TextMeshProUGUI>().text = noText;
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
    }

    public void InitSelling()
    {
        numTransactions = Random.Range(1, 3);
        ChooseVisitor();
    }

    public void ChooseVisitor()
    {
        if (Random.Range(.0f, 2f) <= 1f)
            stateMachine.SetBool("Sell2Hero", true);
        else
            stateMachine.SetBool("Sell2Villain", true);
    }

    public void VisitorLeaves()
    {
        numTransactions--;
        if (numTransactions <= 0)
            Selling(false);
        else
            ChooseVisitor();
    }

    public void AttemptActiveQuest()
    {
        string message = "";
        bool success = quests[activeQuest].AttemptQuest(hero, out message);
        SetText(message);
        if (success) StartNextQuest();
    }

    public void StartNextQuest()
    {
        inventory.UnlockItems();
        Debug.Log("Active Quest = " + activeQuest);
        activeQuest++;
        Debug.Log("Active Quest = " + activeQuest);
        questDisplay.SetQuest(quests[activeQuest]);
    }



    public void ChangeState(GameStates newState)
    {
        //if (state == newState) return;
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
                Selling(false);
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
        if (!stocked && inventory.GetTotalInventory() == 0)
            Selling(false);
    }

    public void NextDialogue()
    {
        stateMachine.SetTrigger("NextDialogue");
    }

    public void AnswerChoice(bool answer)
    {
        if (answer) stateMachine.SetInteger("ChoiceResponse", 1);
        else stateMachine.SetInteger("ChoiceResponse", -1);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }


}
