using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PrerequisiteNode : GoapNode
{
    
    public Prerequisite prerequisite;

    public PrerequisiteNode() : base()
    {
        Color color = Color.orange;
        float width = 1.5f;
        float radius = 8;
        
        style.borderBottomColor = color;
        style.borderTopColor = color;
        style.borderLeftColor = color;
        style.borderRightColor = color;
        style.borderLeftWidth = width;
        style.borderRightWidth = width;
        style.borderBottomWidth = width;
        style.borderTopWidth = width;
        style.borderBottomLeftRadius = radius;
        style.borderBottomRightRadius = radius;
        style.borderTopLeftRadius = radius;
        style.borderTopRightRadius = radius;
        RefreshExpandedState();
    }
}
