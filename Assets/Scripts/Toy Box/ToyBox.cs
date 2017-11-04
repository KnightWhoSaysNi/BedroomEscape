using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToyBox : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject rewardItemParent;
    [SerializeField, Space(5)] private Transform allToyObjectsParent;    
    [SerializeField] private GameObject hiddenObjectsScrollView;
    [SerializeField] private Transform hiddenObjectSlots;
    [SerializeField] private Transform slotPrefab;
    [SerializeField] private Transform slotBufferPrefab;
    [Space(10)]
    [SerializeField] private int numberOfHiddenObjects;
    private List<HiddenObject> allToyObjects;
    private HashSet<HiddenObject> hiddenObjects;
    private Dictionary<HiddenObject, Transform> slotDictionary;

    [Header("Reward Item")]
    [SerializeField] private InventoryItem rewardItem;

    private bool isApplicationClosing;

    public void RegisterObjectSelection(HiddenObject selectedObject)
    {
        if (hiddenObjects.Contains(selectedObject))
        {
            // Selected object is one of the hidden objects
            hiddenObjects.Remove(selectedObject);
            slotDictionary[selectedObject].gameObject.SetActive(false);
            selectedObject.gameObject.SetActive(false);
            slotDictionary.Remove(selectedObject);

            if (hiddenObjects.Count == 0)
            {
                // All hidden objects found
                OnPuzzleSolved();
            }
        }
    }

    private void Awake()
    {
        int childObjectCount = allToyObjectsParent.childCount;
        if (childObjectCount == 0)
        {
            print("Populate toy parent object with toy (hidden) objects!");
            return;
        }
        if (numberOfHiddenObjects > childObjectCount)
        {
            print("Number of hidden objects cannot be larger than the total number of toys in the scene!");
            return;
        }

        allToyObjects = new List<HiddenObject>();
        hiddenObjects = new HashSet<HiddenObject>();
        slotDictionary = new Dictionary<HiddenObject, Transform>();

        FindAllToyObjects(childObjectCount);
        GenerateHiddenObjects();
        DisplayHiddenObjects();
    }

    private void OnApplicationQuit()
    {
        isApplicationClosing = true;
    }

    private void OnEnable()
    {
        //InventoryManager.Instance.SetInventory(false);
        inventory.SetActive(false);

        if (hiddenObjects.Count != 0)
        {
            // Not all hidden object are found yet
            hiddenObjectsScrollView.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (isApplicationClosing)
        {
            // Some game objects are already destroyed on Application.Quit when this script is calling them
            // This stops exception throwing
            return;
        }

        //InventoryManager.Instance.SetInventory(true);
        inventory.SetActive(true);

        if (hiddenObjectsScrollView.activeSelf)
        {
            hiddenObjectsScrollView.SetActive(false);
        }
    }    

    /// <summary>
    /// Finds all toy object from the parent toy object and populates the list.
    /// </summary>
    /// <param name="childObjectCount">Number of children in toy parent.</param>
    private void FindAllToyObjects(int childObjectCount)
    {
        HiddenObject childToyObject;

        for (int i = 0; i < childObjectCount; i++)
        {
            childToyObject = allToyObjectsParent.GetChild(i).GetComponent<HiddenObject>();

            if (childToyObject != null)
            {
                childToyObject.ConnectToyBox(this);
                allToyObjects.Add(childToyObject);
                childToyObject = null;
            }
        }
    }

    /// <summary>
    /// Generates a random collection of hidden objects for the player to find, from the list of all toy objects.
    /// </summary>
    private void GenerateHiddenObjects()
    {
        int hiddenObjectIndex = 0;

        for (int i = 0; i < numberOfHiddenObjects; i++)
        {
            hiddenObjectIndex = Random.Range(0, allToyObjects.Count);
            hiddenObjects.Add(allToyObjects[hiddenObjectIndex]);
            allToyObjects.RemoveAt(hiddenObjectIndex);
        }
    }

    /// <summary>
    /// Displays thumbnails of all hidden objects in the scroll view slots.
    /// </summary>
    private void DisplayHiddenObjects()
    {
        // Start buffer
        GameObject.Instantiate(slotBufferPrefab, hiddenObjectSlots);

        // Instantiate slots and populate them with thumbnails of hidden objects
        foreach (HiddenObject hiddenObject in hiddenObjects)
        {
            Transform slot = GameObject.Instantiate(slotPrefab, hiddenObjectSlots);
            slot.GetComponent<Image>().sprite = hiddenObject.thumbnail;
            slotDictionary.Add(hiddenObject, slot);
        }

        // End buffer
        GameObject.Instantiate(slotBufferPrefab, hiddenObjectSlots);
    }

    private void OnPuzzleSolved()
    {
        hiddenObjectsScrollView.SetActive(false);
        rewardItemParent.SetActive(true);
        AudioManager.Instance.PlayPuzzleSolvedAudio();
        InventoryManager.Instance.RegisterItemAcquisition(rewardItem);
    }
}
