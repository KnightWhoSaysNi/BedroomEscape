using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyBox : MonoBehaviour
{
    [SerializeField] private GameObject toyBoxUnlocked;

    private void OnMouseDown()
    {
        if (InventoryManager.Instance.IsItemSelected(InventoryItemType.PadlockKey))
        {
            toyBoxUnlocked.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
