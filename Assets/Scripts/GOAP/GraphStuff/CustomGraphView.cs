using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomGraphView : GraphView
{
    public CustomGraphView()
    {
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        // Background grid
        GridBackground grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();
        
        RegisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);
        RegisterCallback<ContextualMenuPopulateEvent>(OnContextMenuPopulate);
    }

    
    void OnKeyDown(KeyDownEvent args)
    {
        if (args.keyCode == KeyCode.Space)
        {
            //open node list 
        }
    }
    
    private void OnContextMenuPopulate(ContextualMenuPopulateEvent evt)
    {
        AddActionNode("GetMoney Action", new GetMoneyEffect());
        AddActionNode("Chase Action", new ChaseUnitEffect());
        AddPrerequisiteNode("Money Prerequisite", new MoneyCondition());
        
        
        void AddActionNode(string nodeName, Effect effect)
        {
            evt.menu.AppendAction("Create " + nodeName + " node", a =>
            {
                var newNode = CreateActionNode(a.eventInfo.localMousePosition, effect);
                AddElement(newNode);
            });
        }
        void AddPrerequisiteNode(string nodeName, Condition condition)
        {
            evt.menu.AppendAction("Create " + nodeName + " node", a =>
            {
                var newNode = CreatePrerequisiteNode(a.eventInfo.localMousePosition, condition);
                AddElement(newNode);
            });
        }
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where((nap => 
            nap.direction != startPort.direction && 
            ((nap.node is ActionNode && startPort.node is PrerequisiteNode) || 
             (nap.node is ActionNode && startPort.node is StartNode) || 
             (nap.node is PrerequisiteNode && startPort.node is ActionNode)))).ToList();
    }

    public ActionNode CreateActionNode(Vector2 position, Effect effect)
    {
        var node = new ActionNode();
        node.SetPosition(new Rect(position, position));
        
        GoapAction action = new GoapAction(effect);
        action.SetupNode(node);
        node.action = action;
        return node;
    }

    public StartNode CreateStartNode(Vector2 position)
    {
        StartNode startNode = new StartNode();
        startNode.SetPosition(new Rect(position, position));
        AddElement(startNode);
        return startNode;
    }
    public PrerequisiteNode CreatePrerequisiteNode(Vector2 position, Condition condition)
    {
        var node = new PrerequisiteNode();
        node.SetPosition(new Rect(position, position));
        
        Prerequisite p = new Prerequisite(condition);
        p.SetupNode(node);
        node.prerequisite = p;
        return node;
    }
}