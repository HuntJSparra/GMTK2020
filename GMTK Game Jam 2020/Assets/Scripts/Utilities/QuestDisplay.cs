using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestDisplay : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI descriptionText;
    Quest quest;


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
        UpdateDescription(quest.description);
        quest.OnOffenseChange.AddListener(UpdateOffense);
        quest.OnDefenseChange.AddListener(UpdateDefense);
    }

    public void UpdateOffense(int Atk)
    {
        attackText.text = "Reccomended Attack: " + Atk;
    }

    public void UpdateDefense(int Def)
    {
        defenseText.text = "Reccomended Defense: " + Def;
    }

    public void UpdateDescription(string text)
    {
        //descriptionText.text = text;
    }


}
