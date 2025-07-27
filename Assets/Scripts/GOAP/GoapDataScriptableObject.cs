using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class GoapDataScriptableObject : SerializedScriptableObject
{
    public List<GoapNodeData> goapNodes = new();

    public GoapAction GetRootAction()
    {
        var root = goapNodes.FirstOrDefault(x => x.type == GoapNodeType.Start);
        if (root == null || root.connectedNodes.Count == 0)
            return null;

        return goapNodes.First(x => x.GUID == root.connectedNodes[0]).action;
    }
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
