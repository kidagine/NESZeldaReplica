using System;
using UnityEngine;

[Serializable]
public class Sound
{
    [HideInInspector]
    public AudioSource source;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3)]
    public float pitch;
    public string name;
    public bool loop;
    public bool playOnAwake;
}