using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PuzzleItem : MonoBehaviour
{
    [SerializeField] private InventoryItem inventoryItem;    
    [Space(5)]
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite unselectedSprite;

    private void OnMouseDown()
    {
        AudioManager.Instance.PlayPuzzleSolvedAudio();
        InventoryManager.Instance.RegisterItemAcquisition(inventoryItem, selectedSprite, unselectedSprite);
        Destroy(this.gameObject, 0.1f);
    }
}