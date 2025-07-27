using UnityEngine;

public class StartNode : GoapNode
{
    protected override string outputPortName => "First Action";
    protected override Color portColor => Color.purple;
    
    public StartNode()
    {
        title = "Start Node";

        data.type = GoapNodeType.Start;
        
        Color color = Color.blueViolet;
        style.borderBottomColor = color;
        style.borderTopColor = color;
        style.borderLeftColor = color;
        style.borderRightColor = color;
    }
}