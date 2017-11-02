using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class SwitchablePuzzleView : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private GameObject objectToDeactivate;
    [SerializeField] private AudioClip audioClip;

    [Space(5), Header("Transition"), Space(5)]
    [SerializeField] private GameObject transitionObject;
    [SerializeField] private AudioClip transitionAudioClip;

    private void Start()
    {
        // For disabling the script in editor
    }

    private void OnMouseDown()
    {
        if (this.enabled && !EventSystem.current.IsPointerOverGameObject())
        {
            PuzzleViewManager.Instance.ChangePuzzleViews(objectToActivate, objectToDeactivate, audioClip, transitionObject, transitionAudioClip);
        }
    }
}
