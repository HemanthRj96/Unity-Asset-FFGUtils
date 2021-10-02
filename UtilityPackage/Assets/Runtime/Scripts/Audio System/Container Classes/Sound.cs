using UnityEngine;

[System.Serializable]
public class Sound
{
    public string clipName;
    public AudioClip clip = null;
    public ESoundPlayMode playMode;
    public bool loop;
    [Range(0, 1)]
    public float volume;
    [Range(0, 1)]
    public float pitch;
    [Range(0, 1)]
    public float spatialBlend;
    [Range(0, 100)]
    public float delay;
}