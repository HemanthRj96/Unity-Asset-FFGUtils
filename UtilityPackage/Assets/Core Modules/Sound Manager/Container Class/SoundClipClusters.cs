using System.Collections.Generic;
using UnityEngine;


namespace FickleFrames.Managers.Internal
{
    [CreateAssetMenu(fileName = "Sound_Clip_Cluster", menuName = "-Fickle Frames-/Managers/1. Create New Sound Clip Cluster", order = 0)]
    public class SoundClipClusters : ScriptableObject
    {
        public List<Sound> Sounds = new List<Sound>();
    }
}
