using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace FickleFrames.Audio
{
    [CreateAssetMenu(fileName = "New SoundClips", menuName = "Scriptable Objects/SoundClips")]
    public class SoundClips : ScriptableObject
    {
        public List<Sound> sounds = new List<Sound>();
    }
}
