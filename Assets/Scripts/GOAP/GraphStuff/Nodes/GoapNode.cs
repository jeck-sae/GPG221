using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


public abstract class GoapNode : Node
{
    public GoapNodeData data;
    
    protected virtual string outputPortName { get; }
    protected virtual Color portColor { get; }
    
    public GoapNode()
    {
        data = new GoapNodeData
        {
            GUID = Guid.NewGuid().ToString(),
            position = GetPosition().position
        };

        style.backgroundColor = new Color(0.2f, 0.2f, 0.25f);
        SetPosition(new Rect(transform.position, new Vector2(250, 200)));
        
        
        CreatePort();
        
        float width = 1.5f;
        float radius = 8;
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

    protected void CreateAddPortButtons()
    {
        Button addButton = new Button(() => CreatePort()) { text = "Add Output" };
        Button removeButton = new Button(RemovePort) { text = "Remove Output" };
        mainContainer.Add(addButton);
        mainContainer.Add(removeButton);
    }
    
    protected void AddInputPort()
    {
        var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
        inputPort.portName = "In";
        inputPort.portColor = portColor;
        inputContainer.Add(inputPort);
        
        RefreshPorts();
    }

    public TextField AddTextField(string label, EventCallback<ChangeEvent<string>> callback)
    {
        TextField textField = new TextField();
        textField.label = label;
        textField.RegisterValueChangedCallback(callback);
        mainContainer.Add(textField);
        return textField;
    }

    public Toggle AddToggle(string label, EventCallback<ChangeEvent<bool>> callback)
    {
        Toggle toggle = new Toggle { label = label };
        toggle.RegisterValueChangedCallback(callback);
        mainContainer.Add(toggle);
        return toggle;
    }

    public virtual GoapNodeData SaveState()
    {
        data.position = GetPosition().position;
        
        data.connectedNodes = new List<string>();
        foreach (var portElement in outputContainer.Children())
        {
            if(portElement is not Port port)
                continue;

            var connected = port.connections.FirstOrDefault();
            if(connected?.input.node is not GoapNode node)
                continue;
            
            data.connectedNodes.Add(node.data.GUID);
        }
        
        return data;
    }
    
    public Port CreatePort()
    {
        var port = InstantiatePort(
            Orientation.Horizontal,
            Direction.Output,
            Port.Capacity.Single,
            typeof(float)
        );

        port.portName = outputPortName;
        port.portColor = portColor;
        outputContainer.Add(port);
        RefreshPorts();
        return port;
    }

    public void RemovePort()
    {
        outputContainer.RemoveAt(outputContainer.childCount - 1);
    }
}