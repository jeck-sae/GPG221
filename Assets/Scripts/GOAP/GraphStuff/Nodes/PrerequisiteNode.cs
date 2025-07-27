using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PrerequisiteNode : GoapNode
{
    protected override string outputPortName => "Fallback Action";
    protected override Color portColor => Color.gold;
    
    public Prerequisite prerequisite;

    public PrerequisiteNode() : base()
    {
        data.type = GoapNodeType.Prerequisite;
        
        AddInputPort();
        
        Color color = Color.orange;
        style.borderBottomColor = color;
        style.borderTopColor = color;
        style.borderLeftColor = color;
        style.borderRightColor = color;
        
        RefreshExpandedState();
    }

    public override GoapNodeData SaveState()
    {
        data.prerequisite = prerequisite;
        return base.SaveState();
    }
}
