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

    public GoapAction() { prerequisites = new List<Prerequisite>(); }
    public GoapAction(Effect effect) : this() { this.effect = effect; }

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
        node.AddTextField("Action Node", s => Debug.Log(s));
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
    
    
    public Prerequisite() { fallbackActions = new List<GoapAction>(); }
    public Prerequisite(Condition condition) : this() { this.condition = condition; }


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

    public void SetupNode(PrerequisiteNode node)
    {
        condition.SetupNode(node);
    }
}

[Serializable]
public abstract class Effect
{
    public abstract void DoEffect();
    public abstract void SetupNode(ActionNode node);
}

[Serializable]
public abstract class Condition
{
    public virtual bool CheckCondition()
    {
        return false;
    }
    public abstract void SetupNode(PrerequisiteNode node);
}
