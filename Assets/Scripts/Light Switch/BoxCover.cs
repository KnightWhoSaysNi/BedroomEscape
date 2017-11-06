using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LightSwitch
{    
    public class BoxCover : MonoBehaviour
    {
        [SerializeField] private GameObject closedLightSwitch;
        [SerializeField] private AudioClip boxCloseAudio;
        [SerializeField, Range(0, 100)] private float boxCloseVolumePercent;
        [Space(5)]
        [SerializeField] private AreaTransition lightSwitchAreaTransition;
        [SerializeField] private AreaTransition2D lightSwitchAreaTransition2D;
        [SerializeField] private LightToggle3D lightToggle;
        [SerializeField] private LightToggle2D lightToggle2d;
        private float transitionTime = 0.25f;

        private void OnMouseDown()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                lightSwitchAreaTransition.enabled = false;
                lightSwitchAreaTransition2D.enabled = false;
                lightToggle.enabled = true;
                lightToggle2d.enabled = true;

                // "Close" the ligth switch box
                closedLightSwitch.SetActive(true);
                AudioManager.Instance.PlayAudioClip(boxCloseAudio, boxCloseVolumePercent);
                StartCoroutine(TransitionBackToBedroom());
            }
        }

        private IEnumerator TransitionBackToBedroom()
        {
            // Time to keep the closed light switch visible
            while (transitionTime > 0)
            {
                transitionTime -= Time.deltaTime;
                yield return null;
            }

            // Transition back to bedroom view            
            GameManager.Instance.GoBackToBedroom();
        }
    }
}
