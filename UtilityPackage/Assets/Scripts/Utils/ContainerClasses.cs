using UnityEngine;
using FickleFrames.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip = null;
    public E_SoundPlayMode playMode;
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

[System.Serializable]
public class SoundContainer
{
    public string containerLabel;
    public SoundClips soundClips;
}