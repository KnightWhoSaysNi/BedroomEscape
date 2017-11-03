using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LightSwitch
{
    [RequireComponent(typeof(Collider2D))]
    public class ConnectionPoint : MonoBehaviour
    {
        private Collider2D myCollider2D;

        [SerializeField] private ConnectionSymbol connectionSymbol;
        [SerializeField] private GameObject selectionHighlight;
        private bool canMouseOverHighlight;

        public static event Action<ConnectionPoint> ConnectionPointSelected;

        public ConnectionSymbol ConnectionSymbol
        {
            get
            {
                return connectionSymbol;
            }
        }
        public bool CanMouseOverHighlight
        {
            get
            {
                return canMouseOverHighlight;
            }
            set
            {
                canMouseOverHighlight = value;

                if (!canMouseOverHighlight)
                {
                    selectionHighlight.SetActive(false);
                }
            }
        }

        public void ChangeSelection(bool isSelected)
        {
            selectionHighlight.SetActive(isSelected);
        }        

        /// <summary>
        /// Disables collider2d component and in doing so disables any click events on this game object.
        /// </summary>
        public void DisableConnections()
        {
            myCollider2D.enabled = false;
        }

        private void Awake()
        {
            myCollider2D = GetComponent<Collider2D>();            
        }        

        private void OnMouseDown()
        {            
            if (ConnectionPointSelected != null)
            {
                ConnectionPointSelected(this);
            }
        }

        private void OnMouseEnter()
        {
            if (canMouseOverHighlight)
            {
                selectionHighlight.SetActive(true);
            }
        }

        private void OnMouseExit()
        {
            if (canMouseOverHighlight)
            {
                selectionHighlight.SetActive(false);
            }
        }
    }

    public enum ConnectionSymbol { EarthA, EarthB, NeutralA, NeutralB, LiveA1, LiveB1, LiveA2, LiveB2 }

}