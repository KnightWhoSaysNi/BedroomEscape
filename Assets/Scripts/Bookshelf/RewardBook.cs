using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class RewardBook: MonoBehaviour
{
    private SpriteRenderer mySpriteRenderer;

    [SerializeField] private AudioClip pageTurnAudio;
    [SerializeField, Range(0,100)] private int volumePercent;

    [Header("Page sprites"), Space(3)]
    [SerializeField] private Sprite[] allPagesLightsOff;
    [SerializeField] private Sprite[] allPagesLightsOn;
    private int pageCount;
    private int currentPageIndex;
    private bool isPageTurned;

    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        
        pageCount = allPagesLightsOff.Length;
        Page.PageTurnRequest += TurnPage;
        LightSwitch.LightToggle.LightsToggled += OnLightsToggled;
    }

    private void OnLightsToggled(bool obj)
    {
        mySpriteRenderer.sprite = LightSwitch.LightToggle.IsLightOn ? allPagesLightsOn[currentPageIndex] : allPagesLightsOff[currentPageIndex];
    }

    private void TurnPage(BookPage bookPage)
    {
        isPageTurned = true;

        if (bookPage == BookPage.Left)
        {
            // Turn left page
            currentPageIndex--;

            if (currentPageIndex < 0)
            {
                // Already on first page
                isPageTurned = false;
                currentPageIndex = 0;
            }
        }
        else
        {
            // Turn right page
            currentPageIndex++;

            if (currentPageIndex == pageCount)
            {
                // Already on last page
                isPageTurned = false;
                currentPageIndex = pageCount - 1;
            }
        }

        if (isPageTurned)
        {
            AudioManager.Instance.PlayAudioClip(pageTurnAudio, volumePercent);
            mySpriteRenderer.sprite = LightSwitch.LightToggle.IsLightOn ? allPagesLightsOn[currentPageIndex] : allPagesLightsOff[currentPageIndex];
        }
    }
}
