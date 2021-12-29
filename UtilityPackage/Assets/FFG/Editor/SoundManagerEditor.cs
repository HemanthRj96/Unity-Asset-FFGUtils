using FFG.Managers;
using FFG.Managers.Internal;
using UnityEditor;


[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : BaseEditor<SoundManager>
{
    public override void InspectorUpdate()
    {
        SerializedProperty cluster = GetProperty("_soundClusters");
        SerializedProperty clusterArr(int index) => cluster.GetArrayElementAtIndex(index);


        Heading("Sound Manager Settings");

        Space(5);

        BeginHorizontal();
        if (Button("+"))
            cluster.InsertArrayElementAtIndex(cluster.arraySize);
        if (Button("-") && cluster.arraySize > 0)
            cluster.GetArrayElementAtIndex(cluster.arraySize - 1).DeleteCommand();
        EndHorizontal();

        for(int i =0; i < cluster.arraySize; ++i)
        {
            SoundCluster cacheCluster = (SoundCluster)clusterArr(i).objectReferenceValue;
            string clusterName = cacheCluster == null ? "-" : cacheCluster.ClusterName;
            
            Label($"Cluster Name : {clusterName}");
            PropertyField(clusterArr(i), "Sound Cluster : ", "");
            Space(4);
        }
    }
}
