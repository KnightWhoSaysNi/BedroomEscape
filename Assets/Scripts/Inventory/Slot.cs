using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(Button))]
public class Slot : MonoBehaviour
{
    private Image myImage;
    private Button myButton;

    private InventoryItem inventoryItem;
    private Sprite selectedSprite;
    private Sprite unselectedSprite;
    private bool isSelected;
    private bool isAwoken;

    public InventoryItem InventoryItem
    {
        get
        {
            return inventoryItem;
        }
    }
    public bool IsAwoken
    {
        get
        {
            return isAwoken;
        }
    }

    public void SetSlot(InventoryItem inventoryItem, Sprite selectedSprite, Sprite unselectedSprite)
    {
        this.inventoryItem = inventoryItem;
        this.selectedSprite = selectedSprite;
        this.unselectedSprite = unselectedSprite;

        myImage.sprite = unselectedSprite;
        isSelected = false;
    }

    public void Deselect()
    {
        if (inventoryItem != InventoryItem.Nothing)
        {
            myImage.sprite = unselectedSprite;
            isSelected = false;
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

            inventoryItem = InventoryItem.Nothing;
            isAwoken = true;
        }
    }

    private void OnClick()
    {
        if (inventoryItem != InventoryItem.Nothing)
        {
            isSelected = !isSelected;
            myImage.sprite = isSelected ? selectedSprite : unselectedSprite;

            InventoryManager.Instance.RegisterSlotAction(this);
        }
    }
}
