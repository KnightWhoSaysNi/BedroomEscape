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
        objectToActivate.SetActive(true);
        AudioManager.Instance.PlayAudioClip(resetAudio);

        for (int i = 0; i < objectsToDeactivate.Length; i++)
        {
            objectsToDeactivate[i].SetActive(false);
        }
    }
}
