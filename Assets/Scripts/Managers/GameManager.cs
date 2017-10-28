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
        StartCoroutine(GoToPuzzleArea());
    }   

    public void GoBackToBedroom()
    {
        perspectiveCamera.SetActive(true);
        orthographicCamera.SetActive(false);

        activePuzzleArea.SetActive(false);
        navigation.SetActive(false);
    }

    private void Awake()
    {
        InitializeSingleton();
    }    

    private void SetActivePuzzleArea(PuzzleArea puzzleArea)
    {
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

    private IEnumerator GoToPuzzleArea()
    {
        // Fade out
        while (fadeCanvasGroup.alpha < 1)
        {
            fadeCanvasGroup.alpha += (1 / fadeTime) * Time.deltaTime;
            yield return null;
        }

        // Change cameras
        perspectiveCamera.SetActive(false);
        orthographicCamera.SetActive(true);

        // Activate puzzle area
        activePuzzleArea.SetActive(true);
        navigation.SetActive(true);

        // Fade in
        while (fadeCanvasGroup.alpha > 0)
        {
            fadeCanvasGroup.alpha -= (1 / fadeTime) * Time.deltaTime;
            yield return null;
        }
    }
}

public enum PuzzleArea { BedToys, Bin, Bookshelf, Cupboard, DeskDrawers, Door, LightSwitch, LockBox, Safe, ToyBox, Window }
