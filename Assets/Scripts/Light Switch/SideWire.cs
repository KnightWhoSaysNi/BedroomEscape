using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LightSwitch
{
    [RequireComponent(typeof(Collider2D))]
    public class SideWire : MonoBehaviour
    {
        private static readonly float upscaleMultiplier = 1.5f;

        private Transform myTransform;
        private Vector3 upscale;
        private Vector3 normalScale;

        public SideWireType wireType;
        [SerializeField] private GameObject selectionCircle;
        private bool isSelected;

        public static event Action<SideWire> WireSelected;

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
        }

        public void Deselect()
        {
            isSelected = false;
            selectionCircle.SetActive(false);
            ScaleWire();
        }

        private void Awake()
        {
            myTransform = transform;
            normalScale = myTransform.localScale;
            upscale = myTransform.localScale * upscaleMultiplier;
            WireSelected += OnWireSelected;
        }

        private void OnMouseDown()
        {
            WireSelected(this);
        }

        private void OnWireSelected(SideWire selectedWire)
        {
            if (selectedWire == this)
            {
                isSelected = !isSelected;
            }
            else
            {
                // Other wire was selected so deselect this one if it was previously selected
                isSelected = false;
            }

            ScaleWire();
            selectionCircle.SetActive(isSelected);
        }

        private void ScaleWire()
        {
            myTransform.localScale = isSelected ? upscale : normalScale;
        }
    }

    public enum SideWireType { Earth, Live, Neutral }
}

