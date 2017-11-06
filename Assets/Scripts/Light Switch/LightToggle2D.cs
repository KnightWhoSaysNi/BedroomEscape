using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LightSwitch
{
    [RequireComponent(typeof(Collider2D))]
    public class LightToggle2D : LightToggle
    {
        private void Start()
        {
            // For disabling script in editor
        }

        private void OnMouseDown()
        {
            if (enabled && !EventSystem.current.IsPointerOverGameObject())
            {
                ToggleLights();
            }
        }
    }
}
