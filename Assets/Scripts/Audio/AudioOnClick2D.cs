using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class AudioOnClick2D : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField, Range(0, 100)] private float audioVolumePercent = 100;

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            AudioManager.Instance.PlayAudioClip(audioClip, audioVolumePercent);
        }
    }
}
