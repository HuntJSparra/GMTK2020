using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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
        CompletePurchase,
        DoQuest,
        StartQuest,
        SellingDone,
    }

    public static GameManager instance;
    public GameStates state = GameStates.Start;
    public Inventory inventory;
    public ForSaleStock stock;
    public Wallet wallet;
    public HeroManager hero;
    public DarkLordManager darkLord;
    public TextMeshProUGUI textbox;
    public TextMeshProUGUI overlayTextbox;
    public Button nextButton;
    public Button overlayNextButton;
    public Button yesButton;
    public Button noButton;
    public QuestDisplay questDisplay;
    public Quest[] quests = new Quest[4];

    public UnityEvent OnPurchasingEnter;
    public UnityEvent OnCompletePurchase;
    public UnityEvent OnSellingEnter;
    public UnityEvent OnSellingExit;
    public UnityEvent OnStockingEnter;
    public UnityEvent OnRestockingEnter;
    public UnityEvent HeroAppears;
    public UnityEvent HeroSpeaks;
    public UnityEvent DarkLordAppears;
    public UnityEvent DarkLordSpeaks;
    public UnityEvent DarkLordLeaves;

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

    public void StockPotion(IPotion item)
    {
        bool stocked = stock.AddPotion(item);
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

    public void SetOverlayText(string text)
    {
        overlayTextbox.text = text;
        if (!text.Equals(string.Empty))
            overlayNextButton.gameObject.SetActive(true);
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
        Debug.Log("Init selling");
        numTransactions = Random.Range(1, 3);
        ChooseVisitor();
    }

    public void ChooseVisitor()
    {
        if (Random.Range(.0f, 2f) <= 1f)
        {
            GameManager.instance.HeroAppears.Invoke();
            stateMachine.SetBool("Sell2Hero", true);
        }
        else
        {
            GameManager.instance.DarkLordAppears.Invoke();
            stateMachine.SetBool("Sell2Villain", true);
        }
    }

    public void VisitorLeaves()
    {
        numTransactions--;
        if (numTransactions <= 0)
        {
            Selling(false);
        }
        else
        {
            ChooseVisitor();
        }
    }

    public void AttemptActiveQuest()
    {
        string message = "";
        int reward = 0;
        bool success = quests[activeQuest].AttemptQuest(hero, out reward, out message);

        hero.GainMoney(reward);
        SetOverlayText(message);
        //SetText(message);

        stateMachine.SetBool("QuestSuccess", success);
    }

    public void StartNextQuest()
    {
        inventory.UnlockItems();
        activeQuest++;

        if (activeQuest >= quests.Length)
        {
            SceneManager.LoadScene("ShopScene");
            return;
        }

        questDisplay.SetQuest(quests[activeQuest]);
        //SetText(quests[activeQuest].description);
        SetOverlayText(quests[activeQuest].description);
    }

    public void IncreaseActiveQuestDefense(int attackBoost)
    { quests[activeQuest].IncreaseDefenseNeeded(attackBoost); }


    public void IncreaseActiveQuestAttack(int defenseBoost)
    { quests[activeQuest].IncreaseAttackNeeded(defenseBoost); }

    public Quest GetActiveQuest()
    {
        return quests[activeQuest];
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
            case (GameStates.Stocking):
                OnStockingEnter.Invoke();
                break;
            case (GameStates.Selling):
                OnSellingEnter.Invoke();
                break;
            case (GameStates.Restocking):
                OnRestockingEnter.Invoke();
                Selling(false);
                break;
            case (GameStates.DoQuest):
                AttemptActiveQuest();
                break;
            case (GameStates.StartQuest):
                StartNextQuest();
                break;
            case (GameStates.SellingDone):
                OnSellingExit.Invoke();
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