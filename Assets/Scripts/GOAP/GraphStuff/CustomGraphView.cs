using UnityEditor.Experimental.GraphView;
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

        // Sample Node
        AddElement(CreateNode("Start Node", new Vector2(100, 200)));
        RegisterCallback<ContextualMenuPopulateEvent>(OnContextMenuPopulate);
        RegisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);

    }

    
    void OnKeyDown(KeyDownEvent args)
    {
        if (args.keyCode == KeyCode.Space)
        {
            //open 
        }
        
    }
    
    
    private void OnContextMenuPopulate(ContextualMenuPopulateEvent evt)
    {
        


        void AddActionNode(string nodeName, Effect effect)
        {
            evt.menu.AppendAction("Create " + nodeName + " node", a =>
            {
                Vector2 mousePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
                var newNode = CreateNode(nodeName, mousePosition);
                
                effect.SetupNode(newNode);
                AddElement(newNode);
            });
        }
    }

    
    public ActionNode CreateNode(string title, Vector2 position)
    {
        var node = new ActionNode
        {
            title = title,
            GUID = System.Guid.NewGuid().ToString()
        };
        node.SetPosition(new Rect(position, new Vector2(200, 150)));

        node.RefreshExpandedState();
        node.RefreshPorts();

        return node;
    }
}