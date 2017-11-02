using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToyBoxPadlock : MonoBehaviour
{
    [SerializeField] private GameObject toyBoxLocked;
    [SerializeField] private GameObject toyBoxUnlocked;

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // Cursor over back button
            return;
        }

        if (InventoryManager.Instance.IsItemActive(InventoryItemType.PadlockKey))
        {
            InventoryManager.Instance.UseActiveItem();

            toyBoxUnlocked.SetActive(true);
            toyBoxLocked.SetActive(false);
        }
    }
}
