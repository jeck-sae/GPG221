using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;

public class CustomGraphWindow : EditorWindow
{
    private string fileName = "NewGoapBehaviour";

    private CustomGraphView graphView;
    
    [MenuItem("Tools/GOAP")]
    public static void ShowWindow()
    {
        var window = GetWindow<CustomGraphWindow>();
        window.titleContent = new GUIContent("Graph Tool");
    }

    private void OnEnable()
    {
        graphView = new CustomGraphView { name = "Custom Graph" };
        graphView.StretchToParentSize();

        var toolbar = new Toolbar();
        var fileNameField = new TextField("File Name");
        fileNameField.style.minWidth = 200;
        fileNameField.SetValueWithoutNotify(fileName);
        fileNameField.MarkDirtyRepaint();
        fileNameField.RegisterValueChangedCallback((evt) => fileName = evt.newValue);
        toolbar.Add(fileNameField);
        toolbar.Add(new Button(SaveData) { text = "Save Data" });
        toolbar.Add(new Button(LoadData) { text = "Load Data" });
        
        graphView.CreateStartNode(Vector2.zero);
        
        rootVisualElement.Add(graphView);
        rootVisualElement.Add(toolbar);
    }

    void SaveData()
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return;
        
        GoapDataScriptableObject goapDataSO = ScriptableObject.CreateInstance<GoapDataScriptableObject>();
        
        foreach (var goapNode in graphView.nodes.Cast<GoapNode>())
            if(goapNode != null)
                goapDataSO.goapNodes.Add(goapNode.SaveState());
        
        //from https://github.com/mert-dev-acc/NodeBasedDialogueSystem/blob/master/com.subtegral.dialoguesystem/Editor/GraphSaveUtility.cs
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");
        if (!AssetDatabase.IsValidFolder("Assets/Resources/GOAP"))
            AssetDatabase.CreateFolder("Assets/Resources", "GOAP");
        
        Object loadedAsset = AssetDatabase.LoadAssetAtPath($"Assets/Resources/GOAP/{fileName}.asset", typeof(GoapDataScriptableObject));

        if (loadedAsset == null || !AssetDatabase.Contains(loadedAsset)) 
        {
            AssetDatabase.CreateAsset(goapDataSO, $"Assets/Resources/GOAP/{fileName}.asset");
        }
        else 
        {
            GoapDataScriptableObject container = loadedAsset as GoapDataScriptableObject;
            container.goapNodes = goapDataSO.goapNodes;
            EditorUtility.SetDirty(container);
        }

        AssetDatabase.SaveAssets();
        ///////////////////////////////////////////
    }

    void LoadData()
    {
        var allData = Resources.Load<GoapDataScriptableObject>(Path.Join("GOAP", fileName));
        if (allData == null)
        {
            Debug.LogError("No data found");
            return;
        }
        
        //Clear existing
        graphView.edges.ForEach(x=>graphView.RemoveElement(x));
        graphView.nodes.ForEach(x=>graphView.RemoveElement(x));
        
        //load nodes
        foreach (var nodeData in allData.goapNodes)
        {
            GoapNode node;
            if (nodeData.type == GoapNodeType.Action)
            {
                var effect = nodeData.action.Effect;
                node = graphView.CreateActionNode(nodeData.position, effect);
            }
            else if (nodeData.type == GoapNodeType.Prerequisite)
            {
                var condition = nodeData.prerequisite.condition;
                node = graphView.CreatePrerequisiteNode(nodeData.position, condition);
            }
            else if (nodeData.type == GoapNodeType.Start)
            {
                node = graphView.CreateStartNode(nodeData.position);
            }
            else
            {
                Debug.LogError("Node type not specified.");
                continue;
            }
            
            node.data = nodeData;
            graphView.AddElement(node);
        }
        
        //load connections
        foreach (var nodeData in allData.goapNodes)
        {
            if(nodeData.connectedNodes.Count <= 0)
                continue;
            
            var startNode = graphView.nodes.First(x => x is GoapNode n && n.data.GUID == nodeData.GUID) as GoapNode;
            startNode.RemovePort();
            
            foreach (var connectedGuid in nodeData.connectedNodes)
            {
                var targetNode = graphView.nodes.First(x => x is GoapNode n && n.data.GUID == connectedGuid);
                var startPort = startNode.CreatePort();
                
                var tempEdge = new Edge()
                {
                    output = startPort,
                    input = (Port)targetNode.inputContainer[0]
                };
                tempEdge?.input.Connect(tempEdge);
                tempEdge?.output.Connect(tempEdge);
                graphView.Add(tempEdge);
            }
        }
            
        
    }
    
    
}
