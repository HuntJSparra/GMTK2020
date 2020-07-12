using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestDisplay : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI healthText;
    Quest quest;
    int heroAtk;
    int questAtk;
    int heroDef;
    int questDef;


    public void SetQuest(Quest newQuest)
    {
        if (quest != null)
        {
            quest.OnOffenseChange.RemoveListener(UpdateOffense);
            quest.OnDefenseChange.RemoveListener(UpdateDefense);
        }
        quest = newQuest;
        titleText.text = quest.questName;
        UpdateOffense(quest.GetAttackNeeded());
        UpdateDefense(quest.GetDefenseNeeded());
        quest.OnOffenseChange.AddListener(UpdateOffense);
        quest.OnDefenseChange.AddListener(UpdateDefense);
        UpdateHeroOffense(GameManager.instance.hero.GetOffense());
        UpdateHeroDefense(GameManager.instance.hero.GetDefense());
        UpdateHealthDisplay(GameManager.instance.hero.GetHealth());
        GameManager.instance.hero.OnOffenseChange.AddListener(UpdateHeroOffense);
        GameManager.instance.hero.OnDefenseChange.AddListener(UpdateHeroDefense);
        GameManager.instance.hero.OnHealthChange.AddListener(UpdateHealthDisplay);
    }

    public void UpdateOffense(int Atk)
    {
        questAtk = Atk;
        UpdateAttackDisplay();
    }

    public void UpdateDefense(int Def)
    {
        questDef = Def;
        UpdateDefenseDisplay();
    }

    public void UpdateHeroOffense(int Atk)
    {
        heroAtk = Atk;
        UpdateAttackDisplay();
    }

    public void UpdateHeroDefense(int Def)
    {
        heroDef = Def;
        UpdateDefenseDisplay();
    }

    void UpdateAttackDisplay()
    {
        attackText.text = "Hero Attack: "+heroAtk + "\n Need Attack: "+questAtk;
    }

    void UpdateDefenseDisplay()
    {
        defenseText.text = "Hero Defense: " + heroDef + "\n Need Defense: " + questDef;
    }

    void UpdateHealthDisplay(int health)
    {
        healthText.text = "Hero Health: " + health;
    }



}
