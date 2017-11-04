using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PuzzleViewSwitchData
{
    public GameObject objectToActivate;
    public GameObject objectToDeactivate;
    public AudioClip audioClip;
    public float audioClipVolume;

    public GameObject transitionObject;
    public AudioClip transitionAudioClip;
    public float transitionAudioVolume;

    public InventoryItemType requiredItem;
    public bool isItemLostOnUsage;
}
