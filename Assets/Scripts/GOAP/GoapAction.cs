using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;


//no world state
//no easy way to change values (graph?)

[Serializable]
public class GoapAction
{
    [SerializeReference, ShowInInspector] private List<Prerequisite> prerequisites;
    public List<Prerequisite> Prerequisites
    {
        get => prerequisites; 
        protected set => prerequisites = value;
    }
    
    [SerializeReference, ShowInInspector] private Effect effect;
    public Effect Effect 
    {
        get => effect; 
        protected set => effect = value;
    }
    
    public GoapAction() { Prerequisites = new List<Prerequisite>(); }
    public GoapAction(Effect effect) : this() { this.Effect = effect; }

    
    public (bool success, Effect nextEffect) GetEffect()
    {
        bool hasPrerequisites = true;
        foreach (var prerequisite in Prerequisites)
        {
            var result = prerequisite.CheckPrerequisite();
            if (!result.met)
            {
                if(result.fallbackEffect != null)
                    return (false, result.fallbackEffect);
                
                hasPrerequisites = false;
            }
        }
        
        if (!hasPrerequisites) 
            return (false, null);
        
        return (true, Effect);
    }

    
    public void SetupNode(GoapNode node)
    {
        Effect.SetupNode(node);
    }
}

[Serializable]
public class Prerequisite
{
    [SerializeReference]
    public Condition condition;
    
    [SerializeReference]
    public List<GoapAction> fallbackActions;
    
    
    public Prerequisite() { fallbackActions = new List<GoapAction>(); }
    public Prerequisite(Condition condition) : this() { this.condition = condition; }


    public (bool met, Effect fallbackEffect) CheckPrerequisite()
    {
        if (!condition.CheckCondition())
        {
            foreach (var b in fallbackActions)
            {
                var result = b.GetEffect();
                if (result.nextEffect != null)
                    return (false, result.nextEffect);
            }
            return (false, null);
        }
        return (true, null);
    }

    public void SetupNode(GoapNode node)
    {
        condition.SetupNode(node);
    }
}

[Serializable]
public abstract class Effect
{
    public abstract IEnumerator DoEffect();
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
