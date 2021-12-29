using UnityEditor;
using UnityEngine;


public class BaseEditor<TType> : Editor where TType : Object
{
    public TType Root => (TType)target;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        InspectorUpdate();
        serializedObject.ApplyModifiedProperties();
    }

    public virtual void InspectorUpdate() {  }

    public SerializedProperty GetProperty(string propertyName) 
        => serializedObject.FindProperty(propertyName);

    public void PropertyField(SerializedProperty property) 
        => PropertyField(property, "", "");

    public void PropertyField(SerializedProperty property, string propertyName, string tooltip) 
        => EditorGUILayout.PropertyField(property, new GUIContent(propertyName, tooltip));

    public void Info(string info, MessageType type = MessageType.Info) 
        => EditorGUILayout.HelpBox(info, type);

    public void PropertySlider(SerializedProperty property, float min, float max, string label) 
        => EditorGUILayout.Slider(property, min, max, label);

    public void Space(float val) 
        => GUILayout.Space(val);

    public void Heading(string label)
    {
        var style = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };
        EditorGUILayout.LabelField(label, style, GUILayout.ExpandWidth(true));
    }
    public bool Button(string content) 
        => GUILayout.Button(content);

    public bool Button(string content, float height) 
        => GUILayout.Button(content, GUILayout.Height(height));

    public bool Button(string content, float height, float width) 
        => GUILayout.Button(content, GUILayout.Height(height), GUILayout.Width(width));

    public int DropdownList(string label, int index, string[] choices) 
        => EditorGUILayout.Popup(label, index, choices);

    public void BeginVertical() 
        => EditorGUILayout.BeginVertical();

    public void EndVertical() 
        => EditorGUILayout.EndVertical();

    public void BeginHorizontal() 
        => EditorGUILayout.BeginHorizontal();

    public void EndHorizontal() 
        => EditorGUILayout.EndHorizontal();

    public void Label(string labelContent) 
        => EditorGUILayout.LabelField(labelContent);
}