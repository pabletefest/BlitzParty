using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetAudioVolume(float sliderValue)
    {
        audioMixer.SetFloat("AudioVolume", Mathf.Log10(sliderValue) * 20);
    }
}
