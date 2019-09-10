using System;
using UnityEngine;

class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private Sound[] _sounds = default;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        foreach (Sound s in _sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
            if (s.source.playOnAwake)
            {
                s.source.Play();
            }
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(_sounds, sound => sound.name == name);
        s.source.Play();
    }
}
