using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LightSwitch
{
    public class LightSwitchPuzzle : MonoBehaviour
    {
        private Collider2D myCollider2D;

        [Header("Light Switch Button")]
        [SerializeField] private LightSwitchButton lightSwitchButton;        

        [Header("Box Cover")]
        [SerializeField] private GameObject lightSwitchBoxCover;

        [Header("Side Wires")]
        [SerializeField] private GameObject sideWires;

        [Header("Connection Points")]
        [SerializeField] private ConnectionPoint[] connectionPoints;

        [Space(5), Header("Placed Wires")]
        [SerializeField] private Transform liveWire1;
        [SerializeField] private Transform liveWire2;
        [SerializeField] private GameObject earthWire;
        [SerializeField] private GameObject neutralWire;

        [Header("Alternative Live Wire transforms"), Space(3)]
        [SerializeField] private Vector3 alternativeLiveWire1Position;
        [SerializeField] private Vector3 alternativeLiveWire1Rotation;
        [Space(5)]
        [SerializeField] private Vector3 alternativeLiveWire2Position;
        [SerializeField] private Vector3 alternativeLiveWire2Rotation;
        private bool isLiveWireConnectionFound;
        private bool areLiveWiresInDefaultPosition;

        private SideWire activeWire;
        private ConnectionPoint firstConnectionPoint;
        private ConnectionPoint secondConnectionPoint;
        private bool isWirePlaced;
        private byte numberOfConnections;
        private int numberOfPlacedWires;

        private void Awake()
        {
            myCollider2D = GetComponent<Collider2D>();

            SideWire.WireSelected += OnWireSelected;
            ConnectionPoint.ConnectionPointSelected += OnConnectionPointSelected;
        }

        private void OnMouseDown()
        {            
            if (!EventSystem.current.IsPointerOverGameObject() && InventoryManager.Instance.IsItemActive(InventoryItemType.AllWires))
            {
                InventoryManager.Instance.UseActiveItem();
                sideWires.SetActive(true);

                // Disable additional clicks on this parent object
                myCollider2D.enabled = false;
            }
        }

        private void OnWireSelected(SideWire selectedWire)
        {
            // If selected wire was already selected it is now deselected. Otherwise a new wire is now the active wire
            activeWire = activeWire == selectedWire ? null : selectedWire;
            SetConnectionPointsHighlightValidity(activeWire != null);
            numberOfConnections = 0;

            if (firstConnectionPoint != null)
            {
                firstConnectionPoint.ChangeSelection(false);
            }

            if (secondConnectionPoint != null)
            {
                secondConnectionPoint.ChangeSelection(false);
            }
        }

        private void OnConnectionPointSelected(ConnectionPoint selectedConnectionPoint)
        {
            if (activeWire == null)
            {
                // No wires were selected. Do nothing
                return;
            }

            if (numberOfConnections == 0)
            {
                // First connection
                firstConnectionPoint = selectedConnectionPoint;
                firstConnectionPoint.CanMouseOverHighlight = false; // this also sets selection highlight to false by default so it must go before ChangeSelection(true)
                firstConnectionPoint.ChangeSelection(true);
                numberOfConnections++;
            }
            else
            {
                // Second connection
                numberOfConnections = 0;
                secondConnectionPoint = selectedConnectionPoint;
                firstConnectionPoint.ChangeSelection(false);
                SetConnectionPointsHighlightValidity(false);

                CheckSecondConnection();                
                activeWire.Deselect();
                activeWire = null;
            }
        }    
        
        /// <summary>
        /// Lets every connection point know whether it can use mouse over highlighting action.
        /// </summary>
        private void SetConnectionPointsHighlightValidity(bool canMouseOverHighlight)
        {
            for (int i = 0; i < connectionPoints.Length; i++)
            {
                connectionPoints[i].CanMouseOverHighlight = canMouseOverHighlight;
            }
        }

        private void CheckSecondConnection()
        {
            isWirePlaced = false;

            switch (activeWire.wireType)
            {
                case SideWireType.Earth:
                    CheckEarthWireConnection();
                    break;
                case SideWireType.Live:
                    CheckLiveWireConnection();
                    break;
                case SideWireType.Neutral:
                    CheckNeutralWireConnection();
                    break;
                default:
                    throw new UnityException("There is code for only 3 wires!");
            }

            if (isWirePlaced)
            {
                numberOfPlacedWires++;
                firstConnectionPoint.DisableConnections();
                secondConnectionPoint.DisableConnections();
            }

            if (numberOfPlacedWires == 4)
            {
                // All wires are placed
                OnPuzzleSolved();
            }
        }

        private void CheckEarthWireConnection()
        {
            if ((firstConnectionPoint.ConnectionSymbol == ConnectionSymbol.EarthA && secondConnectionPoint.ConnectionSymbol == ConnectionSymbol.EarthB) ||
                (firstConnectionPoint.ConnectionSymbol == ConnectionSymbol.EarthB && secondConnectionPoint.ConnectionSymbol == ConnectionSymbol.EarthA))
            {
                earthWire.SetActive(true);
                isWirePlaced = true;
            }
        }

        private void CheckLiveWireConnection()
        {
            if ((firstConnectionPoint.ConnectionSymbol == ConnectionSymbol.LiveA1 && secondConnectionPoint.ConnectionSymbol == ConnectionSymbol.LiveB1) ||
                (firstConnectionPoint.ConnectionSymbol == ConnectionSymbol.LiveB1 && secondConnectionPoint.ConnectionSymbol == ConnectionSymbol.LiveA1))
            {
                // Default live wire 1 positioning   
                liveWire1.gameObject.SetActive(true);
                isWirePlaced = true;
            }
            else if ((firstConnectionPoint.ConnectionSymbol == ConnectionSymbol.LiveA2 && secondConnectionPoint.ConnectionSymbol == ConnectionSymbol.LiveB2) ||
                     (firstConnectionPoint.ConnectionSymbol == ConnectionSymbol.LiveB2 && secondConnectionPoint.ConnectionSymbol == ConnectionSymbol.LiveA2))
            {
                // Default live wire 2 positioning   
                liveWire2.gameObject.SetActive(true);
                isWirePlaced = true;
            }
            else if ((firstConnectionPoint.ConnectionSymbol == ConnectionSymbol.LiveA1 && secondConnectionPoint.ConnectionSymbol == ConnectionSymbol.LiveB2) ||
                     (firstConnectionPoint.ConnectionSymbol == ConnectionSymbol.LiveB2 && secondConnectionPoint.ConnectionSymbol == ConnectionSymbol.LiveA1))
            {
                // Alternative live wire 1 positioning   
                liveWire1.transform.SetPositionAndRotation(alternativeLiveWire1Position, Quaternion.Euler(alternativeLiveWire1Rotation));
                liveWire1.gameObject.SetActive(true);
                isWirePlaced = true;
            }
            else if ((firstConnectionPoint.ConnectionSymbol == ConnectionSymbol.LiveA2 && secondConnectionPoint.ConnectionSymbol == ConnectionSymbol.LiveB1) ||
                     (firstConnectionPoint.ConnectionSymbol == ConnectionSymbol.LiveB1 && secondConnectionPoint.ConnectionSymbol == ConnectionSymbol.LiveA2))
            {
                // Alternative live wire 2 positioning   
                liveWire2.transform.SetPositionAndRotation(alternativeLiveWire2Position, Quaternion.Euler(alternativeLiveWire2Rotation));
                liveWire2.gameObject.SetActive(true);
                isWirePlaced = true;
            }
        }

        private void CheckNeutralWireConnection()
        {
            if ((firstConnectionPoint.ConnectionSymbol == ConnectionSymbol.NeutralA && secondConnectionPoint.ConnectionSymbol == ConnectionSymbol.NeutralB) ||
                (firstConnectionPoint.ConnectionSymbol == ConnectionSymbol.NeutralB && secondConnectionPoint.ConnectionSymbol == ConnectionSymbol.NeutralA))
            {
                neutralWire.SetActive(true);
                isWirePlaced = true;
            }
        }

        private void HideSideWires()
        {
            sideWires.SetActive(false);
        }
                
        private void OnPuzzleSolved()
        {
            AudioManager.Instance.PlayPuzzleSolvedAudio();
            InventoryManager.Instance.UseItem(InventoryItemType.AllWires);
            HideSideWires();
            lightSwitchBoxCover.SetActive(true);
            lightSwitchButton.FixLightSwitch();
        }
    }
}

