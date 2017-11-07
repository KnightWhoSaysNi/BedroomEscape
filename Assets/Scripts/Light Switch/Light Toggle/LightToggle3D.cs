using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LightSwitch
{
    [RequireComponent(typeof(Collider))]
    public class LightToggle3D : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private LightToggle lightToggle;

        public void OnPointerClick(PointerEventData eventData)
        {
            lightToggle.ToggleLights();
        }

        private void Start()
        {
            // For disabling script in editor
        }
    }
}
