namespace UnityFoundation.Scripts.BasicAudioManager
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityFoundation.Extensions;

    public class AudioManager : MonoBehaviour
    {
        public  List<AudioClip>                 soundClips        = new();
        public  List<AudioClip>                 musicClips        = new();
        private Dictionary<string, AudioSource> soundAudioSources = new();
        private Dictionary<string, AudioSource> musicAudioSources = new();
        private void OnEnable()
        {
            this.SetupAudio();
        }

        private void SetupAudio()
        {
            this.AddAudioSource(this.soundClips);
            this.AddAudioSource(this.musicClips,false);
        }
        private void AddAudioSource(List<AudioClip>listAudio,bool isSound = true)
        {
            listAudio.ForEach(audioSourceClip =>
            {
                var audioSource = this.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
                if (audioSource == null) return;
                audioSource.clip = audioSourceClip;
                if (isSound)
                {
                    this.soundAudioSources.Add(audioSourceClip.name, audioSource);
                }
                else
                {
                    this.musicAudioSources.Add(audioSourceClip.name, audioSource);
                }
            });
        }

        public void PlaySound(string soundName,bool isLooping = false,bool isPlayImmediately = false)
        {
            if (!this.soundAudioSources.TryGetValue(soundName, out var audioSource)) return;
            if (isPlayImmediately)
            {
                this.PlaySoundInternal(audioSource,isLooping);
                return;
            }
            if(audioSource.isPlaying) return;
            this.PlaySoundInternal(audioSource,isLooping);
        }

        public void PlayMusic(string musicName, bool isPlayImmediately = false,bool isPlayOnly = false)
        {
            if (!this.musicAudioSources.TryGetValue(musicName, out var audioSource)) return;
            if (isPlayOnly) this.musicAudioSources.ForEach(e => e.Value.Stop());
            if (isPlayImmediately)
            {
                this.PlaySoundInternal(audioSource,true);
                return;
            }
            if(audioSource.isPlaying) return;
            this.PlaySoundInternal(audioSource,true);
        }

        private void PlaySoundInternal(AudioSource audioSource,bool isLooping = false)
        {
            audioSource.Play();
            audioSource.loop = isLooping;
        }

        public void StopSoundOrMusic(string soundName)
        {
            if (this.soundAudioSources.TryGetValue(soundName, out var audioSource)) audioSource.Stop();
            if (this.musicAudioSources.TryGetValue(soundName, out var musicSource)) musicSource.Stop();
        }

        public void StopAllSound() => this.soundAudioSources.ForEach(e => this.StopSoundOrMusic(e.Key));

        public void StopAllMusic() => this.musicAudioSources.ForEach(e => this.StopSoundOrMusic(e.Key));

        public void SetAllSoundValue(float value) => this.soundAudioSources.ForEach(e => e.Value.volume = value);

        public void SetAllMusicValue(float value) => this.musicAudioSources.ForEach(e => e.Value.volume = value);

        public void SetSingleSoundOrMusicValue(string soundName, float value)
        {
            if (this.soundAudioSources.TryGetValue(soundName, out var audioSource)) audioSource.volume = value;
            if (this.musicAudioSources.TryGetValue(soundName, out var musicSource)) musicSource.volume = value;
        }
    }
}