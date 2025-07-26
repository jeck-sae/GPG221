using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomGraphWindow : EditorWindow
{
    private string fileName = "New_GOAP_behaviour";
    
    [MenuItem("Tools/GOAP")]
    public static void ShowWindow()
    {
        var window = GetWindow<CustomGraphWindow>();
        window.titleContent = new GUIContent("Graph Tool");
    }

    private void OnEnable()
    {
        var root = rootVisualElement;

        var graphView = new CustomGraphView
        {
            name = "Custom Graph"
        };
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
        
        root.Add(graphView);
        root.Add(toolbar);
    }

    void SaveData()
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return;
        
        
    }

    void LoadData()
    {
        
    }
}
