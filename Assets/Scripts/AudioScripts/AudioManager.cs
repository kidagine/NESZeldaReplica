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
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        foreach (Sound sound in _sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = sound.playOnAwake;
            if (sound.source.playOnAwake)
            {
                sound.source.Play();
            }
        }
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(_sounds, s => s.name == name);
        sound.source.Play();
    }

    public void Stop(string name)
    {
        Sound sound = Array.Find(_sounds, s => s.name == name);
        sound.source.Stop();
    }

    public void PauseEverythingExpect(string name)
    {
        for (int i = 0; i < _sounds.Length; i++)
        {
            if (!_sounds[i].name.Equals(name))
            {
                _sounds[i].source.Pause();
            }
        }
    }

    public void ResumeEverything()
    {
        for (int i = 0; i < _sounds.Length; i++)
        {
            _sounds[i].source.UnPause();
        }
    }

    public bool IsPlaying(string name)
    {
        Sound sound = Array.Find(_sounds, s => s.name == name);
        if (sound.source.isPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
