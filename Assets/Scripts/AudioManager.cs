using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource sound;
    public AudioSource music;

    [Header("Game Music")]
    [SerializeField] private AudioClip gameMusic;

    [Header("UI Sounds")]
    [SerializeField] private AudioClip buttonClick;

    public void SetSoundVolume(float volume)
    {
        sound.volume = volume;
    }
    public void SetMusicVolume(float volume)
    {
        music.volume = volume;
    }
    public void PlayButtonClickSound()
    {
        sound.PlayOneShot(buttonClick);
    }
    public void PlaySound(AudioClip sound)
    {
        this.sound.PlayOneShot(sound);
    }
}
