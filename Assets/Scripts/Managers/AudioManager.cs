using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip puzzleSolved;
    [Space(10)]
    [SerializeField] private AudioClip bookSelected;

    #region - "Singleton" Instance -
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
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
        PlayAudioClip(puzzleSolved);
    }

    public void PlayBookSelectedAudio()
    {
        PlayAudioClip(bookSelected);
    }
    
    public void PlayAudioClip(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    private void Awake()
    {
        InitializeSingleton();

        audioSource = GetComponent<AudioSource>();
    }    

}
