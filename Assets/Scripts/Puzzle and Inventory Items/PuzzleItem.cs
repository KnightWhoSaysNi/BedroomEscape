using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PuzzleItem : MonoBehaviour
{
    [SerializeField] private InventoryItem inventoryItem;

    private void OnMouseDown()
    {
        AudioManager.Instance.PlayPuzzleSolvedAudio();
        InventoryManager.Instance.RegisterItemAcquisition(inventoryItem);
        Destroy(this.gameObject, 0.1f);
    }
}