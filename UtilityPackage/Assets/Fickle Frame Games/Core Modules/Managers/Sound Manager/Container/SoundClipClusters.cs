using System.Collections.Generic;
using UnityEngine;


namespace FFG.Managers.Internal
{
    [CreateAssetMenu(fileName = "Sound_Clip_Cluster", menuName = "Fickle Frame Games/Create New Sound Clip Cluster [Type: ScriptableObject, FileType: Asset]", order = 10)]
    public class SoundClipClusters : ScriptableObject
    {
        public List<Sound> Sounds = new List<Sound>();
    }
}
