using UnityEngine;

namespace FickleFrameGames.Managers.Internal
{
    [System.Serializable]
    public class Sound
    {
        public string ClipName;
        public AudioClip Clip = null;
        public ESoundPlayMode PlayMode;
        public bool Loop;
        [Range(0, 1)]
        public float Volume;
        [Range(-3, 3)]
        public float Pitch = 1;
        [Range(0, 1)]
        public float SpatialBlend;
        [Range(0, 100)]
        public float Delay;
        public bool ShouldPlayOnAwake = false;
    } 
}