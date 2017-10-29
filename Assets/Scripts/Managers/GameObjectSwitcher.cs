using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSwitcher : MonoBehaviour
{
    private GameObject objectToActivate;
    private GameObject objectToDeactivate;
    private AudioClip audioClip;

    private GameObject transitionObject;
    private AudioClip transitionAudioClip;
    [SerializeField] private float transitionTime = 0.2f;
    private float transitionTimer;
    private bool isTransitionInProgress;

    #region - "Singleton" Instance -
    private static GameObjectSwitcher instance;

    public static GameObjectSwitcher Instance
    {
        get
        {
            if (instance == null)
            {
                throw new UnityException("Someone is calling GameObjectSwitcher.Instance before it is set! Change script execution order.");
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

    public void SwitchObjects(GameObject objectToActivate, GameObject objectToDeactivate, AudioClip audioClip, GameObject transitionObject, AudioClip transitionAudioClip)
    {
        if (isTransitionInProgress)
        {
            // Switching objects while transition is in progress is not allowed
            return;
        }

        this.objectToActivate = objectToActivate;
        this.objectToDeactivate = objectToDeactivate;
        this.audioClip = audioClip;

        this.transitionObject = transitionObject;
        this.transitionAudioClip = transitionAudioClip;
        this.transitionTimer = transitionTime;

        if (transitionObject == null)
        {
            // No transition, just switch active objects
            objectToActivate.SetActive(true);
            objectToDeactivate.SetActive(false);
            AudioManager.Instance.PlayAudioClip(audioClip);
        }
        else
        {
            StartCoroutine(TransitionToActivationObject());
        }
    }

    private void Awake()
    {
        InitializeSingleton();
    }

    private IEnumerator TransitionToActivationObject()
    {
        isTransitionInProgress = true;
        // Activate transition object and play transition audio
        objectToDeactivate.SetActive(false);
        transitionObject.SetActive(true);
        AudioManager.Instance.PlayAudioClip(transitionAudioClip);

        while (transitionTimer > 0)
        {
            transitionTimer -= Time.deltaTime;
            yield return null;
        }
        
        // Activate clicked object and play audio
        transitionObject.SetActive(false);
        objectToActivate.SetActive(true);
        AudioManager.Instance.PlayAudioClip(audioClip);

        isTransitionInProgress = false;
    }
}
