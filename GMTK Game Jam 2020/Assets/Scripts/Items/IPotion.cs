using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum potionStat
{
    Attack,
    Defense,
    Health
}

public class IPotion : IItem
{

    public potionStat stat;
    public int statValue;

    public void UsePotion(StatManager user)
    {
        switch(stat)
        {
            case (potionStat.Attack):
                user.IncreaseOffense(statValue);
                break;
            case (potionStat.Defense):
                user.IncreaseDefense(statValue);
                break;
            case (potionStat.Health):
                user.IncreaseHealth(statValue);
                break;
        }
    }

    public void UsePotion(Quest user)
    {
        switch (stat)
        {
            case (potionStat.Attack):
                user.IncreaseDefenseNeeded(statValue);
                break;
            case (potionStat.Defense):
                user.IncreaseAttackNeeded(statValue);
                break;
            case (potionStat.Health):
                break;
        }
    }

}
