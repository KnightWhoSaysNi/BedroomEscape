using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager
{
    public GameObject perspectiveCamera;
    public GameObject orthographicCamera;    

    [Header("Game Menu")]
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GameObject nonGameMenuUI;    
    private bool isGamePaused;

    [Header("End Game Screen")]
    [SerializeField] private GameObject endGameScreen;
    [SerializeField] private float endGameFadeTime;

    [Space(10)]
    [SerializeField] private GameObject backButton;
    [Space(5)]
    [SerializeField] private GameObject bedToys;
    [SerializeField] private GameObject bin;
    [SerializeField] private GameObject boat;
    [SerializeField] private GameObject bookshelf;
    [SerializeField] private GameObject cupboard;
    [SerializeField] private GameObject deskDrawers;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject lightSwitch;
    [SerializeField] private GameObject lockBox;
    [SerializeField] private GameObject safe;
    [SerializeField] private GameObject toyBoxClosed;
    [SerializeField] private GameObject toyBoxOpened;
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
            if (instance == null && !isApplicationClosing)
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

    #region - Game Menu -

    public void Play()
    {
        ToggleGameMenu();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #endregion

    public void GoToEndGameScreen()
    {
        nonGameMenuUI.SetActive(false);
        StartCoroutine(FadeToEndGameScreen());
    }

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
        isGamePaused = true;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleGameMenu();
        }
    }

    private void ToggleGameMenu()
    {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0 : 1;

        gameMenu.SetActive(isGamePaused);
        nonGameMenuUI.SetActive(!isGamePaused);
    }

    private void SetActivePuzzleArea(PuzzleArea puzzleArea)
    {
        if (activePuzzleArea != null)
        {
            previousPuzzleArea = activePuzzleArea;
        }

        switch (puzzleArea)
        {
            case PuzzleArea.BedToys:
                activePuzzleArea = bedToys;
                break;
            case PuzzleArea.Bin:
                activePuzzleArea = bin;
                break;
            case PuzzleArea.Boat:
                activePuzzleArea = boat;
                break;
            case PuzzleArea.Bookshelf:
                activePuzzleArea = bookshelf;
                break;
            case PuzzleArea.Cupboard:
                activePuzzleArea = cupboard;
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
            case PuzzleArea.Safe:
                activePuzzleArea = safe;
                break;
            case PuzzleArea.ToyBoxClosed:
                activePuzzleArea = toyBoxClosed;
                break;
            case PuzzleArea.ToyBoxOpened:
                activePuzzleArea = toyBoxOpened;
                break;
            case PuzzleArea.Window:
                activePuzzleArea = window;
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
        backButton.SetActive(!isBedroomActive);

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

    private IEnumerator FadeToEndGameScreen()
    {        
        fadeCanvasGroup.gameObject.SetActive(true);

        // Fade out
        while (fadeCanvasGroup.alpha < 1)
        {
            fadeCanvasGroup.alpha += (1 / endGameFadeTime) * Time.deltaTime;
            yield return null;
        }

        activePuzzleArea.SetActive(false);        
        endGameScreen.SetActive(true);

        // Fade in in half the time
        while (fadeCanvasGroup.alpha > 0)
        {
            fadeCanvasGroup.alpha -= (2 / endGameFadeTime) * Time.deltaTime;
            yield return null;
        }

        // Deactivate to allow clicks
        fadeCanvasGroup.gameObject.SetActive(false);
    }
}

public enum PuzzleArea { BedToys, Bin, Boat, Bookshelf, Cupboard, DeskDrawers, Door, LightSwitch, LockBox, Safe, ToyBoxClosed, ToyBoxOpened, Window }
