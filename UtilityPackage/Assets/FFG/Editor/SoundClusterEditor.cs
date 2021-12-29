using UnityEditor;
using FFG.Managers.Internal;


[CustomEditor(typeof(SoundCluster))]
public class SoundClusterEditor : BaseEditor<SoundCluster>
{
    public override void InspectorUpdate()
    {
        SerializedProperty clusterName = GetProperty("ClusterName");
        SerializedProperty sounds = GetProperty("Sounds");
        SerializedProperty soundArr(int index) => sounds.GetArrayElementAtIndex(index);

        Heading("Sound Cluster Settings");

        Space(5);

        if (string.IsNullOrEmpty(clusterName.stringValue))
            clusterName.stringValue = Root.name;

        PropertyField(clusterName);

        Space(8);

        BeginHorizontal();

        if (Button("+"))
            sounds.InsertArrayElementAtIndex(sounds.arraySize);
        if (Button("-") && sounds.arraySize > 0)
            sounds.GetArrayElementAtIndex(sounds.arraySize - 1).DeleteCommand();

        EndHorizontal();

        for (int i = 0; i < sounds.arraySize; ++i)
        {
            SerializedProperty clipName = soundArr(i).FindPropertyRelative("ClipName");
            SerializedProperty clip = soundArr(i).FindPropertyRelative("Clip");


            if (string.IsNullOrEmpty(clipName.stringValue))
                clipName.stringValue = clip.objectReferenceValue == null ? "-" : clip.objectReferenceValue.name;
            else if (clipName.stringValue == "-" && clip.objectReferenceValue != null)
                clipName.stringValue = clip.objectReferenceValue.name;

            PropertyField(soundArr(i), $"Sound #{i + 1} {clipName.stringValue}", "");
            Space(5);
        }
    }
}
