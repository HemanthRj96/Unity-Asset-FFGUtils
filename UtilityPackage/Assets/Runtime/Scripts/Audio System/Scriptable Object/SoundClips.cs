using System.Collections.Generic;
using UnityEngine;

namespace FickleFrames
{
    [CreateAssetMenu(fileName = "New SoundClips", menuName = "Scriptable Objects/SoundClips")]
    public class SoundClips : ScriptableObject
    {
        public List<Sound> sounds = new List<Sound>();
    }
}
