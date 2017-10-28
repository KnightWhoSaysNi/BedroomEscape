using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject bedroomLightsOff;
    [SerializeField] private GameObject navigation;
    [Space(5)]
    [SerializeField] private GameObject bookshelf;
    [SerializeField] private GameObject bin;
    [SerializeField] private GameObject deskDrawers;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject lightSwitch;
    [SerializeField] private GameObject cupboard;
    [SerializeField] private GameObject window;
    [SerializeField] private GameObject toyBox;
    [SerializeField] private GameObject bed;
    [SerializeField] private GameObject safe;

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
        activePuzzleArea.SetActive(false);
        bedroomLightsOff.SetActive(true);
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
            case PuzzleArea.Bed:
                activePuzzleArea = bed;
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

        // Deactivate bedroom and find active puzzle area
        bedroomLightsOff.SetActive(false);
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

public enum PuzzleArea { Bookshelf, Bin, DeskDrawers, Door, LightSwitch, Cupboard, Window, ToyBox, Bed, Safe }
