using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameSoundController : MonoBehaviour
{
    public static GameSoundController Instance;

    private AudioSource m_AudioSource;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip audioClip, float volume = 1, bool overlapping = true)
    {
        if (audioClip == null) return;
        if (!this.m_AudioSource.isPlaying | overlapping)
        {
            this.m_AudioSource.PlayOneShot(audioClip, volume);
        }
    }
}
