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
    [SerializeField] private AmbientMusic[] ambientMusic;
    [Space(5)]
    [SerializeField, Range(0.1f, 20)] private float musicFadeDurationSeconds;
    private float[] ambientMusicLengths;
    private bool isFadingOut;
    private bool isFadingIn;
    private int musicIndex;
    private float fadeStartTime;
    private float fadeInterpolationValue;

    [Space(5)]
    [SerializeField] private AudioClip puzzleSolved;
    [SerializeField] private AudioClip bookSelected;

    [Header("Game End")]
    [SerializeField] private float endGameFadeDuration;
    private bool isGameFinished;
    private bool isAudioStopped;

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

    public void RegisterGameFinished()
    {
        isGameFinished = true;
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
        // Upon exiting the door the music fades out and stops
        if (isGameFinished)
        {
            if (!isAudioStopped)
            {
                FadeOut(ambientMusicAudioSource, endGameFadeDuration);
            }

            return;
        }

        // Fade in and out between the ambient music tracks
        if (!isFadingOut && !isFadingIn && Time.time >= fadeStartTime)
        {
            isFadingOut = true;
        }

        if (isFadingOut)
        {
            FadeOut(ambientMusicAudioSource, musicFadeDurationSeconds);
        }

        if (isFadingIn)
        {
            FadeIn(ambientMusicAudioSource);
        }
    }

    private void SetAmbientMusic()
    {
        ambientMusicLengths = new float[ambientMusic.Length];

        for (int i = 0; i < ambientMusic.Length; i++)
        {
            ambientMusicLengths[i] = ambientMusic[i].audioClip.length;
        }

        // Initial settings
        ambientMusicAudioSource.loop = true;
        ambientMusicAudioSource.volume = ambientMusic[0].volume;
        ambientMusicAudioSource.clip = ambientMusic[0].audioClip;
        ambientMusicAudioSource.Play();

        if (ambientMusic[musicIndex].hasFadeOut)
        {
            fadeStartTime = Time.time + ambientMusicLengths[musicIndex];
        }
        else
        {
            fadeStartTime = Time.time + (ambientMusicLengths[musicIndex] - musicFadeDurationSeconds);
        }
    }

    /// <summary>
    /// Finds at what time the music volume should start fading out/in.
    /// </summary>
    private void FindFadeStartTime()
    {
        fadeStartTime += ambientMusicLengths[musicIndex];
    }

    private void FadeOut(AudioSource targetAudioSource, float fadeDuration)
    {
        if (ambientMusic[musicIndex].hasFadeOut && !isGameFinished)
        {
            // Audio clip already has default fading out
            targetAudioSource.volume = 0;
        }
        else
        {
            // Fade out
            fadeInterpolationValue += Time.deltaTime * (1 / fadeDuration);
            targetAudioSource.volume = Mathf.Lerp(ambientMusic[musicIndex].volume, 0, fadeInterpolationValue);
        }

        if (targetAudioSource.volume == 0)
        {
            if (isGameFinished)
            {
                targetAudioSource.Stop();
                isAudioStopped = true;
            }
            else
            {
                isFadingOut = false;
                isFadingIn = true;
                fadeInterpolationValue = 0;

                ChangeMusic();
                FindFadeStartTime();
            }
        }
    }

    private void FadeIn(AudioSource targetAudioSource)
    {
        fadeInterpolationValue += Time.deltaTime * (1 / musicFadeDurationSeconds);
        targetAudioSource.volume = Mathf.Lerp(0, ambientMusic[musicIndex].volume, fadeInterpolationValue);

        if (targetAudioSource.volume == ambientMusic[musicIndex].volume)
        {
            isFadingIn = false;
            fadeInterpolationValue = 0;            
        }
    }

    private void ChangeMusic()
    {
        musicIndex = ++musicIndex % ambientMusic.Length;
        ambientMusicAudioSource.clip = ambientMusic[musicIndex].audioClip;
        ambientMusicAudioSource.Play();
    }
}

[System.Serializable]
public struct AmbientMusic
{
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume;
    public bool hasFadeOut;
}
