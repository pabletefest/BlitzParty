using System;
using UnityEngine;

namespace Services
{
    public class AudioController : ISoundAdapter
    {
        private Sound[] sounds;
        private AudioSource mainThemesSource;
        private AudioSource soundFXSource;

        public AudioController(Sound[] sounds, AudioSource mainThemesSource, AudioSource soundFXSource)
        {
            this.sounds = sounds;
            this.mainThemesSource = mainThemesSource;
            this.soundFXSource = soundFXSource;

            SetMainTheme();
        }

        public bool CheckIsPlaying(string name)
        {
            if (name == null) return false;

            /*
        Sound soundSelected = Array.Find(sounds, sound => sound.Name == name);

        if (soundSelected == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return false;
        }
        */
            bool isPlaying = soundFXSource.name == name;

            return isPlaying;
        }

        public bool CheckMainThemePlaying()
        {
            return mainThemesSource.isPlaying;
        }

        public void PlayMainTheme()
        {
            mainThemesSource.Stop();
            SetMainTheme();
            mainThemesSource.Play();
        }

        public void PlayMinigameTheme(string name)
        {
            if (name == null) return;

            Sound themeSelected = Array.Find(sounds, sound => sound.Name == name);

            if (themeSelected == null)
            {
                Debug.LogWarning("Theme: " + name + " not found.");
                return;
            }

            SetThemeFX(themeSelected);
            mainThemesSource.Play();
        }

        public void PlaySoundFX(string name)
        {
            if (name == null) return;

            Sound soundSelected = Array.Find(sounds, sound => sound.Name == name);

            if (soundSelected == null)
            {
                Debug.LogWarning("Sound: " + name + " not found.");
                return;
            }

            SetSoundFX(soundSelected);
            soundFXSource.Play();
        }

        public string SoundFXPlaying()
        {
            if (soundFXSource.isPlaying) return soundFXSource.name;

            return null;
        }

        public void Stop(string name)
        {
            if (name == null) return;

            //Sound soundSelected = Array.Find(sounds, sound => sound.Name == name);
            bool isPlaying = soundFXSource.name == name;

            /*
        if (soundSelected == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }
        */

            if (!isPlaying)
            {
                Debug.LogWarning("Sound: " + name + " not found.");
                return;
            }

            soundFXSource.Stop();
        }

        public void StopMainTheme()
        {
            if (mainThemesSource.isPlaying) mainThemesSource.Stop();
        }

        public void StopPlayingAll()
        {
            mainThemesSource.Stop();
            soundFXSource.Stop();
        }

        private void SetSoundFX(Sound sound)
        {
            soundFXSource.name = sound.Name;
            soundFXSource.clip = sound.Clip;
            soundFXSource.volume = sound.Volume;
            soundFXSource.pitch = sound.Pitch;
            soundFXSource.loop = sound.Loop;
        }

        private void SetThemeFX(Sound sound)
        {
            mainThemesSource.name = sound.Name;
            mainThemesSource.clip = sound.Clip;
            mainThemesSource.volume = sound.Volume;
            mainThemesSource.pitch = sound.Pitch;
            mainThemesSource.loop = sound.Loop;
        }

        private void SetMainTheme()
        {
            Sound mainThemeSound = Array.Find(sounds, sound => sound.IsMainTheme);
            mainThemesSource.clip = mainThemeSound.Clip;
            mainThemesSource.volume = mainThemeSound.Volume;
            mainThemesSource.pitch = mainThemeSound.Pitch;
            mainThemesSource.loop = mainThemeSound.Loop;
        }
    }
}
