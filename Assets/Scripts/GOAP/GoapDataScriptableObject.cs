using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class GoapDataScriptableObject : SerializedScriptableObject
{
    public List<GoapNodeData> goapNodes = new();
}

public enum GoapNodeType { Start, Action, Prerequisite }

[Serializable]
public class GoapNodeData
{
    public GoapNodeType type;
    public Vector2 position;
    public string GUID;
    public List<string> connectedNodes; 
    [SerializeReference] public GoapAction action;
    [SerializeReference] public Prerequisite prerequisite;
}


public interface IGoapNode
{
    public void SetupNode(GoapNode node);
}
