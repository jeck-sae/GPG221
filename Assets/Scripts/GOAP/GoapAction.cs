using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


//no world state
//no easy way to change values (graph?)

[Serializable]
public class GoapAction : IGoapNode
{
    [SerializeReference, ShowInInspector] private List<Prerequisite> _prerequisites;
    public List<Prerequisite> Prerequisites { get => _prerequisites; protected set => _prerequisites = value; }
    [SerializeReference, ShowInInspector] private Effect _effect;
    public Effect Effect { get => _effect; protected set => _effect = value; }
    
    
    public GoapAction() { Prerequisites = new List<Prerequisite>(); }
    public GoapAction(Effect effect) : this() { this.Effect = effect; }

    public bool TryAction()
    {
        bool hasPrerequisites = true;
        foreach (var prerequisite in Prerequisites)
        {
            if (prerequisite.CheckPrerequisite())
            {
                hasPrerequisites = false;
            }
        }
        
        if (!hasPrerequisites)
            return false;
        
        Effect.DoEffect();
        return true;
    }

    
    public void SetupNode(GoapNode node)
    {
        node.AddTextField("Action Node", s => Debug.Log(s));
        node.AddToggle("test222", s => Debug.Log(s));
        
        Effect.SetupNode(node);
    }
}

[Serializable]
public class Prerequisite : IGoapNode
{
    [SerializeReference]
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

    public void SetupNode(GoapNode node)
    {
        condition.SetupNode(node);
    }
}

[Serializable]
public abstract class Effect
{
    public abstract void DoEffect();
    public abstract void SetupNode(GoapNode node);
}

[Serializable]
public abstract class Condition
{
    public virtual bool CheckCondition()
    {
        return false;
    }
    public abstract void SetupNode(GoapNode node);
}
