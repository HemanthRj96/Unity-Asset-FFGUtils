using System.Collections.Generic;
using UnityEngine;


namespace FickleFrames.Managers.Internal
{
    [CreateAssetMenu(fileName = "New SoundClips", menuName = "Scriptable Objects/SoundClips")]
    public class SoundClipClusters : ScriptableObject
    {
        public List<Sound> sounds = new List<Sound>();
    }
}
