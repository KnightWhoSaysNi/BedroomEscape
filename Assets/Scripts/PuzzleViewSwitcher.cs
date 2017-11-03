using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class PuzzleViewSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private GameObject objectToDeactivate;
    [SerializeField] private AudioClip audioClip;
    [SerializeField, Range(0, 100)] private float audioVolumePercent;

    [Space(5), Header("Transition"), Space(5)]
    [SerializeField] private GameObject transitionObject;
    [SerializeField] private AudioClip transitionAudioClip;
    [SerializeField, Range(0, 100)] private float transitionAudioVolumePercent;

    [Space(10)]
    [Tooltip("Item required for switching puzzle views.")]
    [SerializeField] private InventoryItemType requiredItem;
    [SerializeField] private bool isItemLostOnUsage;

    private PuzzleViewSwitchData puzzleViewSwitchData;

    private void Awake()
    {
        puzzleViewSwitchData = new PuzzleViewSwitchData()
        {
            objectToActivate = this.objectToActivate,
            objectToDeactivate = this.objectToDeactivate,
            audioClip = this.audioClip,
            audioClipVolume = this.audioVolumePercent,

            transitionObject = this.transitionObject,
            transitionAudioClip = this.transitionAudioClip,
            transitionAudioVolume = this.transitionAudioVolumePercent,

            requiredItem = this.requiredItem,
            isItemLostOnUsage = this.isItemLostOnUsage
        };
    }

    private void Start()
    {
        // For disabling the script in editor
    }

    private void OnMouseDown()
    {
        if (this.enabled && !EventSystem.current.IsPointerOverGameObject())
        {
            PuzzleViewManager.Instance.ChangePuzzleViews(puzzleViewSwitchData);
        }
    }
}
