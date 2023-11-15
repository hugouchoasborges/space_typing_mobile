using log;
using sound.settings;
using System.Collections.Generic;
using UnityEngine;

namespace sound
{
    public class SoundController : MonoBehaviour
    {
        private Dictionary<ESoundType, AudioSource> _audioSourceMap = new Dictionary<ESoundType, AudioSource>();
        private ESoundTypeSettingsSO _soundTypeSettings;

        private void Awake()
        {
            _soundTypeSettings = ESoundTypeSettingsSO.Instance;

            // Create one audioSource per SoundType available
            Dictionary<ESoundType, AudioClip> audioClipMap = _soundTypeSettings.GetSoundMap();
            foreach (var soundType in audioClipMap.Keys)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = audioClipMap[soundType];

                _audioSourceMap.Add(soundType, audioSource);
            }
        }

        public AudioClip GetAudioClip(ESoundType soundType) => _soundTypeSettings.GetAudioClip(soundType);

        public void Play(ESoundType sound, float volume = 1, bool playOneShot = true, bool loop = false)
        {
            AudioClip clip = GetAudioClip(sound);

            if (clip == null)
            {
                ELog.LogError(ELogType.SOUND, "Error Playing AudioClip: {0}", sound.ToString());
                return;
            }

            _audioSourceMap[sound].loop = loop;
            if (playOneShot)
            {
                _audioSourceMap[sound].PlayOneShot(clip, volume);
            }
            else
            {
                _audioSourceMap[sound].clip = clip;
                _audioSourceMap[sound].volume = volume;
                _audioSourceMap[sound].Play();
            }
        }

        public void Stop(ESoundType sound)
        {
            AudioClip clip = GetAudioClip(sound);

            // Check if the currently playing clip is the one we want to stop.
            if (_audioSourceMap[sound].clip == clip && _audioSourceMap[sound].isPlaying)
            {
                // Stop the audio playback.
                _audioSourceMap[sound].Stop();
            }
        }
    }
}
