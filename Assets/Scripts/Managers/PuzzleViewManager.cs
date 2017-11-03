using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleViewManager : MonoBehaviour
{
    private GameObject objectToActivate;
    private GameObject objectToDeactivate;
    private AudioClip audioClip;
    private float audioClipVolume;

    private GameObject transitionObject;
    private AudioClip transitionAudioClip;
    private float transitionAudioVolume;
    [SerializeField] private float transitionTime = 0.2f;
    private float transitionTimer;
    private bool isTransitionInProgress;

    /// <summary>
    /// Item required for switching/changing puzzle views.
    /// </summary>
    private InventoryItemType requiredItem;

    #region - "Singleton" Instance -
    private static PuzzleViewManager instance;

    public static PuzzleViewManager Instance
    {
        get
        {
            if (instance == null)
            {
                throw new UnityException("Someone is calling PuzzleViewManager.Instance before it is set! Change script execution order.");
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

    public void ChangePuzzleViews(PuzzleViewSwitchData data)
    {
        if (isTransitionInProgress)
        {
            // Switching objects while transition is in progress is not allowed
            return;
        }

        requiredItem = data.requiredItem;

        if (requiredItem != InventoryItemType.Nothing)
        {
            if (!InventoryManager.Instance.IsItemActive(requiredItem))
            {
                // Player either does't have the required item for changing puzzle views or is not using it
                return;                
            }
            else if (data.isItemLostOnUsage)
            {
                // Player used the item 
                InventoryManager.Instance.UseActiveItem();
            }
        }

        objectToActivate = data.objectToActivate;
        objectToDeactivate = data.objectToDeactivate;
        audioClip = data.audioClip;
        audioClipVolume = data.audioClipVolume;

        transitionObject = data.transitionObject;
        transitionAudioClip = data.transitionAudioClip;
        transitionAudioVolume = data.transitionAudioVolume;

        transitionTimer = transitionTime;

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
