using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Manager
{
    [Header("Inventory Sprites"), Space(3)]
    [SerializeField] private Image inventoryImage;
    [SerializeField] private Sprite inventoryBagSelected;
    [SerializeField] private Sprite inventoryBagUnselected;
    private bool isInventoryOpen;

    [Header("Inventory Slots"), Space(3)]
    [SerializeField] private GameObject slotsParentObject;
    [Space(5)]
    [SerializeField] private Slot[] slots;
    [SerializeField, Space(5)] private Sprite emptySlot;
    private List<InventoryItem> allInventoryItems;
    private Slot activeSlot;
    private InventoryItem activeItem;

    [Header("All Wires"), Space(3)]
    [SerializeField] private InventoryItem allWires;

    [Header("Item combinations"), Space(3)]
    [SerializeField] private ItemCombination[] itemCombinations;    
    private bool isCombinationFound;

    private bool areAllWiresFound;
    private bool areBlackWiresFound;
    private bool areRedWiresFound;
    private bool isEarthWireFound;

    #region - "Singleton" Instance -
    private static InventoryManager instance;

    public static InventoryManager Instance
    {
        get
        {
            if (instance == null && !isApplicationClosing)
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

    public bool IsItemInInventory(InventoryItemType itemType)
    {
        for (int i = 0; i < allInventoryItems.Count; i++)
        {
            if (allInventoryItems[i].inventoryItemType == itemType)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsItemActive(InventoryItemType itemType)
    {
        return activeItem.inventoryItemType == itemType;
    }

    public void UseActiveItem()
    {
        RemoveInventoryItem(activeItem.inventoryItemType);
        SortSlots();
    }

    public void UseItem(InventoryItemType itemType)
    {
        RemoveInventoryItem(itemType);
    }

    public void RegisterSlotAction(Slot actionSlot)
    {
        if (activeSlot == null)
        {
            // Nothing was selected previously
            activeSlot = actionSlot;
            activeItem = actionSlot.inventoryItem;
        }
        else
        {
            // Something was selected previously. Deselect the active slot
            activeSlot.Deselect();

            if (activeSlot == actionSlot)
            {
                // Previously active item was deselected
                activeSlot = null;
                activeItem.inventoryItemType = InventoryItemType.Nothing;
            }
            else 
            {
                // New item was selected
                activeSlot = actionSlot;

                // Go through all item combinations and see if there is a match
                isCombinationFound = false;
                for (int i = 0; i < itemCombinations.Length; i++)
                {
                    if (activeItem.inventoryItemType == itemCombinations[i].firstItem &&                // Already clicked item (active item) is the first item
                        actionSlot.inventoryItem.inventoryItemType == itemCombinations[i].secondItem)   // Newly clicked item (activeSlot's item) is the second item
                    {
                        // Item combination found. By default second item is "destroyed" and replaced with the "gainedItem"                        
                        isCombinationFound = true;
                        InventoryItem gainedItem = itemCombinations[i].gainedItem;
                        allInventoryItems.Remove(actionSlot.inventoryItem);     
                        allInventoryItems.Add(gainedItem);
                        activeSlot.SetSlot(gainedItem);
                        activeItem = gainedItem;
                        AudioManager.Instance.PlayPuzzleSolvedAudio();
                        break;
                    }
                }

                if (!isCombinationFound)
                {
                    // No combination found so just make the newly selected item the active item
                    activeItem = actionSlot.inventoryItem;
                }
                else if (!areAllWiresFound)
                {
                    // Combination found, but all wires weren't previously found. So check if they have now
                    CheckIfWiresFound();
                    CheckIfAllWiresFound();
                }
            }
        }
    }

    public void RegisterItemAcquisition(InventoryItem acquiredItem)
    {
        // Find empty slot for the acquired item
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].inventoryItem.inventoryItemType == InventoryItemType.Nothing)
            {                
                slots[i].SetSlot(acquiredItem);
                break;
            }
        }

        allInventoryItems.Add(acquiredItem);

        // Check if earth wire is found
        if (acquiredItem.inventoryItemType == InventoryItemType.EarthWire)
        {
            isEarthWireFound = true;
            CheckIfAllWiresFound();
        }
    }

    public void SetInventory(bool isOpen)
    {
        isInventoryOpen = isOpen;

        inventoryImage.sprite = isInventoryOpen ? inventoryBagSelected : inventoryBagUnselected;
        slotsParentObject.SetActive(isInventoryOpen);

        if (!isInventoryOpen && activeSlot != null)
        {
            // Inventory is closing, so deselect active item and item slot
            activeSlot.Deselect();
            activeSlot = null;
            activeItem.inventoryItemType = InventoryItemType.Nothing;
        }
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        SetInventory(isInventoryOpen);
    }

    private void Awake()
    {
        InitializeSingleton();
        allInventoryItems = new List<InventoryItem>();
    }

    private void Start()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Awake();
        }
    }

    /// <summary>
    /// Checks if black or red wires were found/created.
    /// </summary>
    private void CheckIfWiresFound()
    {
        if (activeItem.inventoryItemType == InventoryItemType.BlackWires)
        {
            areBlackWiresFound = true;
        }
        else if (activeItem.inventoryItemType == InventoryItemType.RedWires)
        {
            areRedWiresFound = true;
        }
    }

    /// <summary>
    /// Removes individual wires from the player's inventory and replaces them with a single "AllWires" item.
    /// </summary>
    private void CheckIfAllWiresFound()
    {
        if (areBlackWiresFound && isEarthWireFound && areRedWiresFound)
        {
            areAllWiresFound = true;
            RemoveInventoryItem(InventoryItemType.BlackWires);
            RemoveInventoryItem(InventoryItemType.EarthWire);
            RemoveInventoryItem(InventoryItemType.RedWires);
            RemoveInventoryItem(InventoryItemType.WireSnips);

            SortSlots();
            
            RegisterItemAcquisition(allWires);
            AudioManager.Instance.PlayPuzzleSolvedAudio();
        }
    }

    private void RemoveInventoryItem(InventoryItemType itemToRemove)
    {        
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].inventoryItem.inventoryItemType == itemToRemove)
            {
                allInventoryItems.Remove(slots[i].inventoryItem);
                slots[i].ClearSlot(emptySlot);
                return;
            }
        }
    }

    private void SortSlots()
    {
        // Go through all slots and find non-empty ones
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < allInventoryItems.Count)
            {
                slots[i].SetSlot(allInventoryItems[i]);
            }
            else
            {
                slots[i].ClearSlot(emptySlot);            
            }
        }
    }
}

public enum InventoryItemType { Nothing, BlackWires, BlackWireSpool, DoorKey, EarthWire, PadlockKey, RedWires, RedWireSpool, Screwdriver, SpyGlass, WireSnips, AllWires }
