using UnityEditor;
using UnityEngine;


public class CustomInspector<TType> : Editor where TType : Object 
{
    public TType root => (TType)target;
    public SerializedProperty getProperty(string propertyName) => serializedObject.FindProperty(propertyName);
    public void propertyField(SerializedProperty property, string propertyName, string tooltip) => EditorGUILayout.PropertyField(property, new GUIContent(propertyName, tooltip));
    public void info(string info, MessageType type = MessageType.Info) => EditorGUILayout.HelpBox(info, type);
    public void propertySlider(SerializedProperty property, float min, float max, string label)=> EditorGUILayout.Slider(property, min, max, label);
    public void space(float val) => GUILayout.Space(val);
    public void heading(string label)
    {
        var style = new GUIStyle(GUI.skin.label) 
        { 
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };
        EditorGUILayout.LabelField(label, style, GUILayout.ExpandWidth(true));
    }
    public bool button(string content, float height = 10) => GUILayout.Button(content, GUILayout.Height(height));
    public int dropdownList(string label, int index, string[] choices) => EditorGUILayout.Popup(label, index, choices);
}
