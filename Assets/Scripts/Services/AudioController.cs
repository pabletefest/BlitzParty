using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : ISoundAdapter
{
    private Sound[] sounds;
    private AudioSource mainThemeSource;
    private AudioSource soundFXSource;

    public AudioController(Sound[] sounds, AudioSource mainThemeSource, AudioSource soundFXSource)
    {
        this.sounds = sounds;
        this.mainThemeSource = mainThemeSource;
        this.soundFXSource = soundFXSource;

        Sound mainThemeSound = Array.Find(sounds, sound => sound.IsMainTheme);
        mainThemeSource.name = mainThemeSound.Name; 
        mainThemeSource.clip = mainThemeSound.Clip;
        mainThemeSource.volume = mainThemeSound.Volume;
        mainThemeSource.pitch = mainThemeSound.Pitch;
        mainThemeSource.loop = mainThemeSound.Loop;
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
        return mainThemeSource.isPlaying;
    }

    public void PlayMainTheme()
    {
        if (!mainThemeSource.isPlaying) mainThemeSource.Play();
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
        if (mainThemeSource.isPlaying) mainThemeSource.Stop();
    }

    public void StopPlayingAll()
    {
        mainThemeSource.Stop();
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
}
