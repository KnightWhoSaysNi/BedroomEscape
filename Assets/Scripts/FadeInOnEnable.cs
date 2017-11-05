using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeInOnEnable : MonoBehaviour
{
    [SerializeField] private CanvasGroup fader;
    [SerializeField, Range(0, 10)] private float fadeInStartDelay = 0;
    [SerializeField, Range(0, 10)] private float fadeInDurationSeconds;
    [Space(10)]
    [SerializeField] private GameObject nextFadeInObject;

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        // Start delay
        while (fadeInStartDelay > 0)
        {
            fadeInStartDelay -= Time.deltaTime;
            yield return null;
        }

        // Fade in
        while (fader.alpha < 1)
        {
            fader.alpha += Time.deltaTime * (1 / fadeInDurationSeconds);
            yield return null;
        }

        if (nextFadeInObject != null)
        {
            nextFadeInObject.SetActive(true);
        }
    }
}
