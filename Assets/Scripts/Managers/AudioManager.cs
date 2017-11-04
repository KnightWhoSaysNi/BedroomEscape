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
    [SerializeField] private AudioClip[] ambientMusic;
    [SerializeField, Range(0, 1)] private float ambientMusicVolume;
    [SerializeField, Range(0, 20)] private float musicFadeLengthSeconds;
    private float[] ambientMusicLengths;
    private bool isFadingOut;
    private bool isFadingIn;
    private int musicIndex;
    private float fadeStartTime;
    private float fadeEndTime;
    private float fadeInterpolationValue;

    [Space(5)]
    [SerializeField] private AudioClip puzzleSolved;
    [SerializeField] private AudioClip bookSelected;

    private bool isSoundOn;

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

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        ambientMusicAudioSource.mute = !isSoundOn;
        soundEffectsAudioSource.mute = !isSoundOn;
    }

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
        isSoundOn = true;

        SetAmbientMusic();
    }

    private void Update()
    {
        if (!isFadingOut && !isFadingIn && Time.time >= fadeStartTime)
        {
            isFadingOut = true;
        }

        if (isFadingOut)
        {
            FadeOut(ambientMusicAudioSource);
        }

        if (isFadingIn && Time.time <= fadeEndTime)
        {
            FadeIn(ambientMusicAudioSource);
        }
    }

    private void SetAmbientMusic()
    {
        ambientMusicLengths = new float[ambientMusic.Length];

        for (int i = 0; i < ambientMusic.Length; i++)
        {
            ambientMusicLengths[i] = ambientMusic[i].length;
        }

        ambientMusicAudioSource.loop = true;
        ambientMusicAudioSource.volume = ambientMusicVolume;
        ambientMusicAudioSource.clip = ambientMusic[0];
        ambientMusicAudioSource.Play();

        FindFadeTimes();        
    }

    /// <summary>
    /// Finds at what times the music volume should start/end fading.
    /// </summary>
    private void FindFadeTimes()
    {
        fadeStartTime = Time.time + (ambientMusicLengths[musicIndex] - musicFadeLengthSeconds);
        fadeEndTime = fadeStartTime + 2 * musicFadeLengthSeconds;
    }

    private void FadeOut(AudioSource targetAudioSource)
    {
        fadeInterpolationValue += Time.deltaTime * (1 / musicFadeLengthSeconds);
        targetAudioSource.volume = Mathf.Lerp(ambientMusicVolume, 0, fadeInterpolationValue);

        if (targetAudioSource.volume == 0)
        {
            isFadingOut = false;
            isFadingIn = true;
            fadeInterpolationValue = 0;

            ChangeMusic();
        }
    }

    private void FadeIn(AudioSource targetAudioSource)
    {
        fadeInterpolationValue += Time.deltaTime * (1 / musicFadeLengthSeconds);
        targetAudioSource.volume = Mathf.Lerp(0, ambientMusicVolume, fadeInterpolationValue);

        if (targetAudioSource.volume == ambientMusicVolume)
        {
            isFadingIn = false;
            fadeInterpolationValue = 0;

            FindFadeTimes();
        }
    }

    private void ChangeMusic()
    {
        musicIndex = ++musicIndex % ambientMusic.Length;
        ambientMusicAudioSource.clip = ambientMusic[musicIndex];
        ambientMusicAudioSource.Play();
    }
}
