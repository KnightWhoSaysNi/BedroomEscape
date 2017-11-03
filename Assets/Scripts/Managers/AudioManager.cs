using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : Manager
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource ambientMusicAudioSource;
    [SerializeField] private AudioSource soundEffectsAudioSource;

    [Header("Often Used Audio Clips")]
    [SerializeField] private AudioClip ambientMusic; // TODO set both clips
    [SerializeField, Range(0, 100)] private float ambientMusicVolume;
    [SerializeField, Space(5)] private AudioClip puzzleSolved;
    [SerializeField] private AudioClip bookSelected;

    #region - "Singleton" Instance -
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null && !isApplicationClosing)
            {
                throw new UnityException("Someone is calling AudioManager.Instance before it is set! Change script execution order.");
            }

            return instance;
        }
    }

    private void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public void PlayPuzzleSolvedAudio()
    {
        PlayAudioClip(puzzleSolved, 45);
    }

    public void PlayBookSelectedAudio()
    {
        PlayAudioClip(bookSelected);
    }
    
    public void PlayAudioClip(AudioClip audioClip, float volumePercent = 100)
    {
        if (audioClip != null)
        {
            soundEffectsAudioSource.clip = audioClip;
            soundEffectsAudioSource.volume = volumePercent / 100f;
            soundEffectsAudioSource.Play();
        }
    }

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        ambientMusicAudioSource.loop = true;
        ambientMusicAudioSource.volume = ambientMusicVolume / 100;
        ambientMusicAudioSource.clip = ambientMusic;
        ambientMusicAudioSource.Play();
    }
}
