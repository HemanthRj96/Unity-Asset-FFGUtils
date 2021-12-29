using System.Collections.Generic;
using UnityEngine;


namespace FFG.Managers.Internal
{
    [CreateAssetMenu(fileName = "Sound Cluster", menuName = "FFG/Create New Sound Cluster [Type: ScriptableObject, FileType: Asset]")]
    public class SoundCluster : ScriptableObject
    {
        [SerializeField]
        public string ClusterName;
        [SerializeField]
        public Sound[] Sounds;
    }
}
