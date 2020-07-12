using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatManager : MonoBehaviour
{

    protected int offenseStat;
    protected int defenseStat;
    protected int healthStat;
    protected int money;

    public IntEvent OnOffenseChange;
    public IntEvent OnDefenseChange;
    public IntEvent OnHealthChange;
    public IntEvent OnMoneyChange;

    public int offenseStartingStat;
    public int defenseStartingStat;
    public int healthStartingStat;
    public int moneyStartingStat;

    // Start is called before the first frame update
    void Start()
    {
        SetOffense(offenseStartingStat);
        SetDefense(defenseStartingStat);
        SetHealth(healthStartingStat);
        SetMoney(moneyStartingStat);
    }

    public int GetOffense() { return offenseStat; }
    public int GetDefense() { return defenseStat; }
    public int GetHealth() { return healthStat; }
    public int GetMoney() { return money; }

    public void SetOffense(int newOffense)
    {
        offenseStat = newOffense;
        OnOffenseChange.Invoke(offenseStat);
    }

    public void ChangeOffense(int value)
    {
        offenseStat += value;
        OnOffenseChange.Invoke(offenseStat);
    }

    public void IncreaseOffense(int value) { ChangeOffense(Mathf.Abs(value)); }
    public void DecreaseOffense(int value) { ChangeOffense(-Mathf.Abs(value)); }

    public void SetDefense(int newDefense)
    {
        defenseStat = newDefense;
        OnDefenseChange.Invoke(defenseStat);
    }

    public void ChangeDefense(int value)
    {
        defenseStat += value;
        OnDefenseChange.Invoke(defenseStat);
    }

    public void IncreaseDefense(int value) { ChangeDefense(Mathf.Abs(value)); }
    public void DecreaseDefense(int value) { ChangeDefense(-Mathf.Abs(value)); }

    public void SetHealth(int newHealth)
    {
        healthStat = newHealth;
        OnHealthChange.Invoke(healthStat);
    }

    public void ChangeHealth(int value)
    {
        healthStat += value;
        OnHealthChange.Invoke(healthStat);
        if (healthStat <= 0) OutOfHealth();
    }

    public void IncreaseHealth(int value) { ChangeHealth(Mathf.Abs(value)); }
    public void DecreaseHealth(int value) { ChangeHealth(-Mathf.Abs(value)); }

    public void SetMoney(int newMoney)
    {
        money = newMoney;
        OnMoneyChange.Invoke(money);
    }

    public void ChangeMoney(int value)
    {
        money += value;
        OnMoneyChange.Invoke(money);
    }

    public void IncreaseMoney(int value) { ChangeMoney(Mathf.Abs(value)); }
    public void DecreaseMoney(int value) { ChangeMoney(-Mathf.Abs(value)); }

    public virtual void OutOfHealth()
    {
        Debug.Log("Game Over, Hero is dead");
    }
}
