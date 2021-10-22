using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FickleFrames
{
    /// <summary>
    /// Use this Audio Manager singleton class for controlling all the audio in the game
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        #region Internals

        [Space(5)]
        [Header("-Audio Manager Settings-")]
        [SerializeField]
        private List<SoundContainer> soundContainers = new List<SoundContainer>();
        
        private Dictionary<string, Dictionary<string, Sound>> soundLookup = new Dictionary<string, Dictionary<string, Sound>>();


        private new void Awake()
        {
            base.Awake();
            bootstrapLookups();
        }


        /// <summary>
        /// Method to bootstrap the sound lookup container
        /// </summary>
        private void bootstrapLookups()
        {
            foreach (SoundContainer container in soundContainers)
            {
                Dictionary<string, Sound> tempSounds = new Dictionary<string, Sound>();
                foreach (Sound sound in container.soundClips.sounds)
                    tempSounds.Add(sound.clipName, sound);
                soundLookup.Add(container.containerTag, tempSounds);
            }
        }


        /// <summary>
        /// Returns true if sound found
        /// </summary>
        private bool soundFetcher(string containerTag, string clipName, out Sound sound)
        {
            if (soundLookup.ContainsKey(containerTag))
            {
                if (soundLookup[containerTag].ContainsKey(clipName))
                {
                    sound = soundLookup[containerTag][clipName];
                    return true;
                }
                else
                {
                    sound = null;
                    soundClipError(clipName);
                    return false;
                }
            }
            else
            {
                sound = null;
                containerError(containerTag);
                return false;
            }
        }


        /// <summary>
        /// Plays sound
        /// </summary>
        private void internalSoundPlayer(AudioSource source, Sound sound)
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
                case ESoundPlayMode.Play:
                    source.Play();
                    break;
                case ESoundPlayMode.PlayOneShot:
                    source.PlayOneShot(source.clip, source.volume);
                    break;
                case ESoundPlayMode.PlayDelayed:
                    source.PlayDelayed(sound.delay);
                    break;
            }
        }


        private void containerError(string containerTag)
        {
            Debug.LogError($"CONTAINER TAG : {containerTag} NOT FOUND!!");
        }


        private void soundClipError(string clipName)
        {
            Debug.LogError($"SOUND CLIP : {clipName} NOT FOUND!!");
        }


        #endregion Internals


        /// <summary>
        /// Overloaded version of PlaySound, this is slightly faster than the previous function
        /// </summary>
        /// <param name="containerTag">Target container label</param>
        /// <param name="clipName">Name of the clip to be played</param>
        /// <param name="source">Reference to self</param>
        public void PlaySound(string containerTag, string clipName, GameObject source = null)
        {
            Sound sound = null;
            AudioSource audioSource = null;

            if (!soundFetcher(containerTag, clipName, out sound))
                return;

            // Check if the source object has a AudioSource component, if not play sound using PlaySoundAt
            if (source != null)
            {
                if (source.TryGetComponent(out audioSource))
                    internalSoundPlayer(audioSource, sound);
                else
                {
                    audioSource = source.AddComponent<AudioSource>();
                    internalSoundPlayer(audioSource, sound);
                }
            }
            internalSoundPlayer(audioSource, sound);
        }


        /// <summary>
        /// Call this method to play sound at a specific location
        /// </summary>
        /// <param name="location">Target GameObject where the sound has to be played</param>
        /// <param name="clipName">Audio clip to be played</param>
        public void PlaySoundAt(Vector3 location, string containerTag, string clipName)
        {
            Sound sound = null;
            // Try and find the target sound object
            if (!soundFetcher(containerTag, clipName, out sound))
                return;
            PlaySoundAt(location, sound.clip);
        }


        /// <summary>
        /// Call this method to play sound at a specific gameObject location
        /// </summary>
        /// <param name="target">Target GameObject where the sound has to be played</param>
        /// <param name="clipName">Audio clip to be played</param>
        public void PlaySoundAt(GameObject target, string containerTag, string clipName)
        {
            PlaySoundAt(target.transform.position, containerTag, clipName);
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
        /// Call this method to play sound at a specific gameObject location
        /// </summary>
        /// <param name="target">Target GameObject where the sound has to be played</param>
        /// <param name="clip">Audio clip to be played</param>
        public void PlaySoundAt(GameObject target, AudioClip clip)
        {
            PlaySoundAt(target.transform.position, clip);
        }


        /// <summary>
        /// Returns a sound object
        /// </summary>
        /// <param name="soundName">Target sound name</param>
        public Sound GetSoundObject(string containertag, string clipName)
        {
            Sound sound = null;
            soundFetcher(containertag, clipName, out sound);
            return sound;
        }
    }
}