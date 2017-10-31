using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Button))]
public class Slot : MonoBehaviour
{
    private Image myImage;
    private Button myButton;

    [HideInInspector]
    public InventoryItem inventoryItem;
    private bool isSelected;
    private bool isAwoken;
        
    public bool IsAwoken
    {
        get
        {
            return isAwoken;
        }
    }

    public void Awake()
    {
        // A slot can be called from the InventoryManager before the actual game object becomes active, in which case this method was already called
        // The object wasn't technically awoken, but the code from this method was run
        if (!isAwoken)
        {
            myImage = GetComponent<Image>();
            myButton = GetComponent<Button>();
            myButton.onClick.AddListener(OnClick);

            inventoryItem = new InventoryItem();

            isAwoken = true;
        }
    }

    public void SetSlot(InventoryItem inventoryItem)
    {
        this.inventoryItem = inventoryItem;
        myImage.sprite = inventoryItem.unselectedSprite;
        isSelected = false;
    }

    public void Deselect()
    {
        if (inventoryItem.inventoryItemType != InventoryItemType.Nothing)
        {
            myImage.sprite = inventoryItem.unselectedSprite;
            isSelected = false;
        }
    }

    public void ClearSlot(Sprite emptySprite)
    {
        inventoryItem.inventoryItemType = InventoryItemType.Nothing;
        myImage.sprite = emptySprite;        
    }

    private void OnClick()
    {
        if (inventoryItem.inventoryItemType != InventoryItemType.Nothing)
        {
            isSelected = !isSelected;
            myImage.sprite = isSelected ? inventoryItem.selectedSprite : inventoryItem.unselectedSprite;

            InventoryManager.Instance.RegisterSlotAction(this);
        }
    }
}
