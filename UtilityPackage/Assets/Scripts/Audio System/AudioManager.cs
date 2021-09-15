using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FickleFrames.Audio
{
    /// <summary>
    /// Use this Audio Manager singleton class for controlling all the audio in the game
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField]
        private List<SoundContainer> soundContainers = new List<SoundContainer>();

        /// <summary>
        /// Play a sound from a list, loaded through SoundClips
        /// </summary>
        /// <param name="clipName">Name of the clip to played</param>
        /// <param name="source">Reference to self</param>
        public void PlaySound(string clipName, GameObject source = null)
        {
            Sound cachedSound = GetSoundObject(clipName);
            // If cachedSound is null then the clip does not exist
            if (cachedSound == null)
            {
                Debug.LogWarning("AUDIO CLIP NOT FOUND!");
                return;
            }
            // Check if there's a valid gameObject with AudioSource component, if not play sound from self
            AudioSource cachedSource = null;
            if (source != null && source.TryGetComponent(out cachedSource))
                localSoundPlayer(cachedSource, cachedSound);
            else
                localSoundPlayer(null, cachedSound);
        }

        /// <summary>
        /// Overloaded version of PlaySound, this is slightly faster than the previous function
        /// </summary>
        /// <param name="containerLabel">Target container label</param>
        /// <param name="clipName">Name of the clip to be played</param>
        /// <param name="source">Reference to self</param>
        public void PlaySound(string containerLabel, string clipName, GameObject source = null)
        {
            // Try and find the container with the label
            SoundContainer container = getSoundContainer(containerLabel);
            if (container == null)
            {
                Debug.LogWarning("AUDIO CONTAINER NOT FOUND!");
                return;
            }
            // Try and find the target sound object
            Sound cachedSound = null;
            foreach (var sound in container.soundClips.sounds)
                if (sound.name == clipName)
                    cachedSound = sound;
            if (cachedSound == null)
            {
                Debug.LogWarning("AUDIO CLIP NOT FOUND!");
                return;
            }
            // Check if the source object has a AudioSource component, if not play sound using PlaySoundAt
            AudioSource cachedSource = null;
            if (source != null && source.TryGetComponent(out cachedSource))
                localSoundPlayer(cachedSource, cachedSound);
            else
                localSoundPlayer(null, cachedSound);
        }

        /// <summary>
        /// Call this method to play sound at a specific location
        /// </summary>
        /// <param name="target">Target GameObject where the sound has to be played</param>
        /// <param name="clip">Audio clip to be played</param>
        public void PlaySoundAt(GameObject target, AudioClip clip)
        {
            AudioSource.PlayClipAtPoint(clip, target.transform.position);
        }

        /// <summary>
        /// Call this method to play sound at a specific location
        /// </summary>
        /// <param name="target">Target GameObject where the sound has to be played</param>
        /// <param name="clipName">Audio clip to be played</param>
        public void PlaySoundAt(GameObject target, string clipName)
        {
            Sound cachedSound = null;
            // Try and find the target sound object
            foreach (SoundContainer container in soundContainers)
            {
                foreach (Sound sound in container.soundClips.sounds)
                    if (sound.name == clipName)
                        cachedSound = sound;
            }
            // If cachedSound is null then the clip does not exist
            if (cachedSound == null)
            {
                Debug.LogWarning("AUDIO CLIP NOT FOUND!");
                return;
            }

            AudioSource.PlayClipAtPoint(cachedSound.clip, target.transform.position);
        }

        /// <summary>
        /// Call this method to play sound at a specific location
        /// </summary>
        /// <param name="location">Target location where the sound has to be played</param>
        /// <param name="clip">Audio clip to be played</param>
        public void PlaySoundAt(Vector3 location, AudioClip clip)
        {
            AudioSource.PlayClipAtPoint(clip, location);
        }

        /// <summary>
        /// Call this method to play sound at a specific location
        /// </summary>
        /// <param name="location">Target GameObject where the sound has to be played</param>
        /// <param name="clipName">Audio clip to be played</param>
        public void PlaySoundAt(Vector3 location, string clipName)
        {
            Sound cachedSound = null;
            // Try and find the target sound object
            foreach (SoundContainer container in soundContainers)
            {
                foreach (Sound sound in container.soundClips.sounds)
                    if (sound.name == clipName)
                        cachedSound = sound;
            }
            // If cachedSound is null then the clip does not exist
            if (cachedSound == null)
            {
                Debug.LogWarning("AUDIO CLIP NOT FOUND!");
                return;
            }
            AudioSource.PlayClipAtPoint(cachedSound.clip, location);
        }

        /// <summary>
        /// Returns a sound object
        /// </summary>
        /// <param name="soundName">Target sound name</param>
        public Sound GetSoundObject(string soundName)
        {
            foreach (SoundContainer container in soundContainers)
                foreach (Sound sound in container.soundClips.sounds)
                    if (sound.name == soundName)
                        return sound;
            return null;
        }


        /// <summary>
        /// Helper method to play a sound in different modes
        /// </summary>
        /// <param name="source">Target audio source</param>
        /// <param name="sound">Target audio clip</param>
        private void localSoundPlayer(AudioSource source, Sound sound)
        {
            if (source == null)
            {
                PlaySoundAt(transform.position, sound.clip);
                return;
            }

            source.loop = sound.loop;
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.spatialBlend = sound.spatialBlend;

            switch (sound.playMode)
            {
                case E_SoundPlayMode.Play:
                    source.Play();
                    break;
                case E_SoundPlayMode.PlayOneShot:
                    source.PlayOneShot(source.clip, source.volume);
                    break;
                case E_SoundPlayMode.PlayDelayed:
                    source.PlayDelayed(sound.delay);
                    break;
            }
        }

        /// <summary>
        /// Helper method to that return the target container
        /// </summary>
        /// <param name="containerLabel">Target container label</param>
        private SoundContainer getSoundContainer(string containerLabel)
        {
            foreach (var temp in soundContainers)
                if (temp.containerLabel == containerLabel)
                    return temp;
            return null;
        }
    }
}