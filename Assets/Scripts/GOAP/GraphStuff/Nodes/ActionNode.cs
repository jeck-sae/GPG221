using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = System.Random;

public class ActionNode : GoapNode
{
    protected override string outputPortName => "Prerequisite";
    protected override Color portColor => Color.green;

    public GoapAction action;

    public ActionNode()
    {
        data.type = GoapNodeType.Action;
        
        CreateAddPortButtons();
        AddInputPort();
        
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
        action.Prerequisites.Clear();
        
        foreach (var portElement in outputContainer.Children())
        {
            if (portElement is not Port port) continue;
            var connected = port.connections.FirstOrDefault();
            if (connected?.input.node is not PrerequisiteNode node) continue;
            
            action.Prerequisites.Add(node.prerequisite);
        }

        return base.SaveState();
    }
    
}
