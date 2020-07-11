using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{

    public string QuestName;
    public Vector2 attackRange;
    public Vector2 defenseRange;
    public string successMsg;
    public string failMsgSword;
    public string failMsgArmor;
    int attackNeeded;
    int defenseNeeded;
    bool completed = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        attackNeeded = (int) Random.Range(attackRange.x, attackRange.y);
        defenseNeeded = (int)Random.Range(defenseRange.x, defenseRange.y);
    }

    public int GetAttackNeeded() { return attackNeeded; }
    public int GetDefenseNeeded() { return defenseNeeded; }

    public bool AttemptQuest(HeroManager hero, out string message)
    {
        if (completed)
        {
            message = "Quest already completed!";
            return true;
        }
        if (hero.GetOffense() < attackNeeded || hero.GetDefense() < defenseNeeded)
        {
            if (hero.GetOffense() < attackNeeded) message = failMsgSword;
            else message = failMsgArmor;
            hero.DecreaseHealth(-1);
            return false;
        }
        message = successMsg;
        return true;
    }

    
}
