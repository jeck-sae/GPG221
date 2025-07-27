using System;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class ActionNode : GoapNode
{
    protected override string outputPortName => "Prerequisite";
    protected override Color portColor => Color.green;

    public GoapAction action;

    public ActionNode() : base()
    {
        AddInputPort();

        data.type = GoapNodeType.Action;
        
        Color color = Color.forestGreen;
        style.borderBottomColor = color;
        style.borderTopColor = color;
        style.borderLeftColor = color;
        style.borderRightColor = color;
        
        RefreshExpandedState();
    }

    public override GoapNodeData SaveState()
    {
        data.action = action;
        return base.SaveState();
    }
}
