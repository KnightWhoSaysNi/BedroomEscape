using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyBoxOpened : OnEnableMessages
{
    protected override void OnEnable()
    {
        if (InventoryManager.Instance.IsItemInInventory(InventoryItemType.SpyGlass))
        {
            base.OnEnable();
        }
    }
}
