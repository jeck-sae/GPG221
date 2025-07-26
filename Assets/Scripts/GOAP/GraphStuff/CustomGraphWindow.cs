using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomGraphWindow : EditorWindow
{
    [MenuItem("Tools/Custom Graph Tool")]
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
        root.Add(graphView);
    }
}