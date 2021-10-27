using UnityEngine;

namespace FickleFrames.Managers.Internal
{
    [System.Serializable]
    public class Sound
    {
        public string clipName;
        public AudioClip clip = null;
        public ESoundPlayMode playMode;
        public bool loop;
        [Range(0, 1)]
        public float volume;
        [Range(-3, 3)]
        public float pitch = 1;
        [Range(0, 1)]
        public float spatialBlend;
        [Range(0, 100)]
        public float delay;
        public bool shouldPlayOnAwake = false;
    } 
}