using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleViewResetter : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private GameObject[] objectsToDeactivate;
    [SerializeField] private AudioClip resetAudio;

    private void OnDisable()
    {   
        // Play audio clip if default object isn't already active
        if (!objectToActivate.activeSelf)
        {
            AudioManager.Instance.PlayAudioClip(resetAudio);
        }

        objectToActivate.SetActive(true);

        for (int i = 0; i < objectsToDeactivate.Length; i++)
        {
            objectsToDeactivate[i].SetActive(false);
        }
    }
}
