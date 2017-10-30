using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Sprites"), Space(3)]
    [SerializeField] private Image inventoryImage;
    [SerializeField] private Sprite selectedInventoryBag;
    [SerializeField] private Sprite unselectedInventoryBag;
    private bool isInventoryOpen;

    [Header("Inventory Slots"), Space(3)]
    [SerializeField] private GameObject slotsParentObject;
    [Space(5)]
    [SerializeField] private Slot[] slots;
    [SerializeField] private Sprite emptySlot;
    private Slot activeSlot;
    private InventoryItem activeItem;

    #region - "Singleton" Instance -
    private static InventoryManager instance;

    public static InventoryManager Instance
    {
        get
        {
            if (instance == null)
            {
                throw new UnityException("Someone is calling InventoryManager.Instance before it is set! Change script execution order.");
            }

            return instance;
        }
    }

    private void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public void RegisterSlotAction(Slot actionSlot)
    {
        if (activeSlot == actionSlot)
        {
            // Previously active slot was deactivated
            activeSlot = null;
            activeItem = InventoryItem.Nothing;
        }
        else
        {
            // New slot was activated. Deselect the previous one (if there was one) and update the activeSlot
            if (activeSlot != null)
            {
                activeSlot.Deselect();
            }
            activeSlot = actionSlot;
            activeItem = actionSlot.InventoryItem;
        }        
    }

    public void RegisterItemAcquisition(InventoryItem acquiredItem, Sprite selectedSprite, Sprite unselectedSprite)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].InventoryItem == InventoryItem.Nothing)
            {
                // First empty slot
                slots[i].SetSlot(acquiredItem, selectedSprite, unselectedSprite);
                break;
            }
        }
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        inventoryImage.sprite = isInventoryOpen ? selectedInventoryBag : unselectedInventoryBag;
        slotsParentObject.SetActive(isInventoryOpen);

        if (!isInventoryOpen)
        {
            // Inventory is closed, so deselect active item slot
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].Deselect();
                activeSlot = null;
                activeItem = InventoryItem.Nothing;
            }
        }
    }

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Awake();
        }
    }
}

public enum InventoryItem { Nothing, BlackWires, BlackWireSpool, DoorKey, EarthWire, PadlockKey, RedWires, RedWireSpool, Screwdriver, SpyGlass, WireSnips, AllWires }
