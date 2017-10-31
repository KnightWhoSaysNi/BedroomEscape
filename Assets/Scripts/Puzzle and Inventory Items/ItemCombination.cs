using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemCombination
{
    public string title;
    [Space(5)]
    public InventoryItemType firstItem;
    public InventoryItemType secondItem;
    [Space(10)]
    public InventoryItem gainedItem;
}
