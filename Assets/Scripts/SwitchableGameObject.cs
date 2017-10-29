using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class SwitchableGameObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private GameObject objectToDeactivate;
    [SerializeField] private AudioClip audioClip;

    [Space(5), Header("Transition"), Space(5)]
    [SerializeField] private GameObject transitionObject;
    [SerializeField] private AudioClip transitionAudioClip;

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            GameObjectSwitcher.Instance.SwitchObjects(objectToActivate, objectToDeactivate, audioClip, transitionObject, transitionAudioClip);
        }
    }
}
