using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject perspectiveCamera;
    public GameObject orthographicCamera;

    [Space(10)]
    [SerializeField] private GameObject navigation;
    [Space(5)]
    [SerializeField] private GameObject bedToys;
    [SerializeField] private GameObject bin;
    [SerializeField] private GameObject bookshelf;
    [SerializeField] private GameObject cupboard;
    [SerializeField] private GameObject deskDrawers;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject lightSwitch;
    [SerializeField] private GameObject lockBox;
    [SerializeField] private GameObject safe;
    [SerializeField] private GameObject toyBox;
    [SerializeField] private GameObject window;
    [Space(10)]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField, Range(0, 1)] private float fadeTime;

    private GameObject activePuzzleArea;
    private GameObject previousPuzzleArea;

    #region - "Singleton" Instance -
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                throw new UnityException("Someone is calling GameManager.Instance before it is set! Change script execution order.");
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

    public void GoToPuzzleArea(PuzzleArea puzzleArea)
    {
        SetActivePuzzleArea(puzzleArea);        
        StopAllCoroutines();
        StartCoroutine(ChangeAreas(false));
    }   

    public void GoBackToBedroom()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeAreas(true));        
    }

    private void Awake()
    {
        InitializeSingleton();
    }    

    private void SetActivePuzzleArea(PuzzleArea puzzleArea)
    {
        if (activePuzzleArea != null)
        {
            previousPuzzleArea = activePuzzleArea;
        }

        switch (puzzleArea)
        {
            case PuzzleArea.Bookshelf:
                activePuzzleArea = bookshelf;
                break;
            case PuzzleArea.Bin:
                activePuzzleArea = bin;
                break;
            case PuzzleArea.DeskDrawers:
                activePuzzleArea = deskDrawers;
                break;
            case PuzzleArea.Door:
                activePuzzleArea = door;
                break;
            case PuzzleArea.LightSwitch:
                activePuzzleArea = lightSwitch;
                break;
            case PuzzleArea.LockBox:
                activePuzzleArea = lockBox;
                break;
            case PuzzleArea.Cupboard:
                activePuzzleArea = cupboard;
                break;
            case PuzzleArea.Window:
                activePuzzleArea = window;
                break;
            case PuzzleArea.ToyBox:
                activePuzzleArea = toyBox;
                break;
            case PuzzleArea.BedToys:
                activePuzzleArea = bedToys;
                break;
            case PuzzleArea.Safe:
                activePuzzleArea = safe;
                break;
            default:
                throw new UnityException("No code for this enum!");
        }
    }

    private IEnumerator ChangeAreas(bool isBedroomActive)
    {
        // Activate to block additional clicks
        fadeCanvasGroup.gameObject.SetActive(true);

        // Fade out
        while (fadeCanvasGroup.alpha < 1)
        {
            fadeCanvasGroup.alpha += (1 / fadeTime) * Time.deltaTime;
            yield return null;
        }

        // Change cameras
        perspectiveCamera.SetActive(isBedroomActive);
        orthographicCamera.SetActive(!isBedroomActive);

        // Activate puzzle area
        activePuzzleArea.SetActive(!isBedroomActive);
        navigation.SetActive(!isBedroomActive);

        // Deactivate last puzzle area, if there was one
        if (previousPuzzleArea != null)
        {
            previousPuzzleArea.SetActive(false);
        }

        // If going back to the bedroom, reset active and previous puzzle areas for the next iteration
        if (isBedroomActive)
        {
            activePuzzleArea = null;
            previousPuzzleArea = null;
        }

        // Fade in
        while (fadeCanvasGroup.alpha > 0)
        {
            fadeCanvasGroup.alpha -= (1 / fadeTime) * Time.deltaTime;
            yield return null;
        }

        // Deactivate to allow clicks
        fadeCanvasGroup.gameObject.SetActive(false);
    }
}

public enum PuzzleArea { BedToys, Bin, Bookshelf, Cupboard, DeskDrawers, Door, LightSwitch, LockBox, Safe, ToyBox, Window }
