using FFG.GameManager;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : BaseEditor<GameManager>
{
    SerializedProperty subManagerContainer;
    ReorderableList list;

    public override void InspectorUpdate()
    {
        Heading("Game Manager Settings");

        Space(8);
        Info(
            @"A sub manager is segmented game logic which controls a game state based on certain constraints. Each sub manager object " +
            @"has to be inherited from a 'SubGameManager' class. The game object can be PREFABS or ON-SCENE objects. When they are " +
            @"ON-SCENE object then make sure they have 'DoNotDestroyOnLoad' as TRUE. These intricacies are not implemented in this custom " +
            @"inspector since backend settings should be modified directly and implementing this functionality would only complicate intializing " +
            @"default settings. The order of the list isn't important however the first element in the list is going to be the " +
            @"entry point for the game manager."
            );

        Space(15);
        list.DoLayoutList();
    }

    private void OnEnable()
    {
        subManagerContainer = serializedObject.FindProperty("_subManagers");

        list = new ReorderableList(serializedObject, subManagerContainer, true, true, true, true);
        list.drawElementCallback = DrawListItems;
        list.drawHeaderCallback = DrawHeader;
    }

    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

        EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), $"Unique Name : ");
        EditorGUI.PropertyField(
            new Rect(rect.x + 87.5f, rect.y, 125, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("SubManagerName"),
            GUIContent.none
        );

        EditorGUI.LabelField(new Rect(rect.x + 220, rect.y, 100, EditorGUIUtility.singleLineHeight), $"Manager : ");

        EditorGUI.PropertyField(
            new Rect(rect.x + 280, rect.y, 150, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("SubManager"),
            GUIContent.none
        );
    }

    void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Sub-Game Managers");
    }
}
