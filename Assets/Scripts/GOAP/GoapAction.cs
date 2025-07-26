using System;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

//no world state
//no easy way to change values (graph?)

[Serializable]
public class GoapAction
{
    [OdinSerialize] List<Prerequisite> prerequisites;
    [OdinSerialize] Effect effect;
    
    public bool TryAction()
    {
        bool hasPrerequisites = true;
        foreach (var prerequisite in prerequisites)
        {
            if (prerequisite.CheckPrerequisite())
            {
                hasPrerequisites = false;
            }
        }
        
        if (!hasPrerequisites)
            return false;
        
        effect.DoEffect();
        return true;
    }


    public void SetupNode(ActionNode node)
    {
        node.AddTextField("test", s => Debug.Log(s));
        node.AddToggle("test222", s => Debug.Log(s));
        
        effect.SetupNode(node);
    }
}

[Serializable]
public class Prerequisite
{
    [OdinSerialize]
    public Condition condition;
    
    [SerializeReference]
    public List<GoapAction> fallbackActions;

    public bool CheckPrerequisite()
    {
        if (!condition.CheckCondition())
        {
            foreach (var b in fallbackActions)
            {
                if (b.TryAction())
                    return true;
            }
        }
        return false;
    }
}

[Serializable]
public class Effect
{
    public virtual void DoEffect()
    {
        
    }
}

[Serializable]
public class Condition
{
    public virtual bool CheckCondition()
    {
        return false;
    }
}

[Serializable]
public class GetMoneyEffect : Effect
{
    public int amount;
    public override void DoEffect()
    {
        //money += amount
    }
}

public class MoneyCondition : Condition
{
    public int minMoney;
    public override bool CheckCondition()
    {
        return false;
    }
}