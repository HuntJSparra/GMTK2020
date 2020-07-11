using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Wallet : MonoBehaviour
{
    public int money = 100;
    public TextMeshProUGUI moneyText;

    private void Start()
    {
        moneyText.text = "Money: " + money;
    }

    public void EarnMoney(int amount)
    {
        money += Mathf.Abs(amount);
        moneyText.text = "Money: " + money;
    }

    public void LoseMoney(int amount)
    {
        money -= Mathf.Abs(amount);
        moneyText.text = "Money: " + money;
    }

}
