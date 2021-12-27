using FFG.Managers;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(PrefabManager))]
public class PrefabManagerEditor : BaseEditor<PrefabManager>
{
    private void InspectorUpdate()
    {
        SerializedProperty prefabs = GetProperty("_prefabContainer");

        if (Button("Load all prefabs in this project", 25))
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                bool flag = false;

                for (int i = 0; i < prefabs.arraySize; ++i)
                    if (obj.Equals(prefabs.GetArrayElementAtIndex(i).FindPropertyRelative("Prefab").objectReferenceValue as GameObject))
                    {
                        flag = true;
                        break;
                    }

                if (flag == false)
                {
                    int index = prefabs.arraySize;

                    prefabs.InsertArrayElementAtIndex(index);
                    prefabs.GetArrayElementAtIndex(index).FindPropertyRelative("PrefabName").stringValue
                        = obj.name;
                    prefabs.GetArrayElementAtIndex(index).FindPropertyRelative("Prefab").objectReferenceValue
                        = obj;
                }
            }
        }

        Space(10);

        Heading("Loaded prefabs");

        Space(5);

        for (int i = 0; i < prefabs.arraySize; ++i)
        {
            var elem = prefabs.GetArrayElementAtIndex(i);

            BeginHorizontal();

            PropertyField(elem.FindPropertyRelative("PrefabName"));
            PropertyField(elem.FindPropertyRelative("Prefab"));
            if (Button("X", 17.5f, 17.5f))
            {
                prefabs.GetArrayElementAtIndex(i).DeleteCommand();
                continue;
            }

            EndHorizontal();
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        InspectorUpdate();
        serializedObject.ApplyModifiedProperties();
    }
}
