using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "BlitzParty/SoundSO", order = 1)]
public class Sound : ScriptableObject 
{
	public string Name;

	public AudioClip Clip;

	[Range(0f, 1f)]
	public float Volume;

	[Range(0.1f, 3f)]
	public float Pitch;


	public bool Loop;

	public bool IsMainTheme;
}
