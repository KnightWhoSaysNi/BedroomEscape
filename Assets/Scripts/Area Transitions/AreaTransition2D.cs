using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class AreaTransition2D : MonoBehaviour
{
    [SerializeField] private PuzzleArea puzzleArea;

    [Space(10)]
    [SerializeField] private InventoryItemType requiredItem;
    [SerializeField] private string missingItemMessage = string.Empty;

    private void OnMouseDown()
    {
        if (enabled && !EventSystem.current.IsPointerOverGameObject())
        {
            if (requiredItem != InventoryItemType.Nothing && !InventoryManager.Instance.IsItemActive(requiredItem))
            {
                // Item is required and player doesn't have it yet
                DialogManager.Instance.DisplayMessage(missingItemMessage);
            }
            else
            {
                // Item isn't required for area transition OR the player has the required item
                GameManager.Instance.GoToPuzzleArea(puzzleArea);
            }
        }
    }
}
