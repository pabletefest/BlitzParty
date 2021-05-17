using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    [SerializeField]
    private Database database;

    [SerializeField]
    private GameObject animationMusic;

    [SerializeField]
    private GameObject animationMute;

    public AudioMixer audioMixer;

    public void SetMusicVolume(float sliderValue)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        database.SaveMusicVolume(sliderValue);
        if (sliderValue < 0.001)
        {
            animationMusic.SetActive(false);
            animationMute.SetActive(true);
        }
        else 
        {
            animationMusic.SetActive(true);
            animationMute.SetActive(false);
        }
    }

    public void SetSFXVolume(float sliderValue)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
        database.SaveSFXVolume(sliderValue);
    }
}
