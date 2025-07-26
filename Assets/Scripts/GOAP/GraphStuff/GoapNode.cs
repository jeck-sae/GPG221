using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


public abstract class GoapNode : Node
{
    public string GUID;
    public Vector2 position;

    public GoapNode()
    {
        title = "Custom Node";
        style.backgroundColor = new Color(0.2f, 0.2f, 0.25f);
    
        // Add ports
        var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
        inputPort.portName = "In";
        inputPort.portColor = this is ActionNode ? Color.green : Color.yellow;
        inputContainer.Add(inputPort);
        CreatePort();
        ////////////
    
    
        Button addButton = new Button(CreatePort) { text = "Add Output" };
        Button removeButton = new Button(RemovePort) { text = "Remove Output" };
        mainContainer.Add(addButton);
        mainContainer.Add(removeButton);
    
        SetPosition(new Rect(transform.position, new Vector2(250, 200)));
        
    
        // Refresh layout
        RefreshExpandedState();
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
        Toggle toggle = new Toggle();
        toggle.label = label;
        toggle.RegisterValueChangedCallback(callback);
        mainContainer.Add(toggle);
        return toggle;
    }
    
    
    
    
    private void CreatePort()
    {
        var port = InstantiatePort(
            Orientation.Horizontal,
            Direction.Output,
            Port.Capacity.Single,
            typeof(float)
        );

        port.portName = this is ActionNode ? "Prerequisite" : "Fallback Action";
        port.portColor = this is ActionNode ? Color.green : Color.yellow;
        outputContainer.Add(port);
        RefreshPorts();
    }

    private void RemovePort()
    {
        outputContainer.RemoveAt(outputContainer.childCount - 1);
    }
}