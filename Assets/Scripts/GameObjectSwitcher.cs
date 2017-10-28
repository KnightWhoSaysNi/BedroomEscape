using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GameObjectSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private GameObject objectToDeactivate;

    [SerializeField] private GameObject transitionObject;
    [SerializeField] private float transitionTime;
    private float transitionTimer;

    private void Awake()
    {
        transitionTimer = transitionTime;
    }

    private void OnMouseDown()
    {
        if (transitionObject == null)
        {
            objectToActivate.SetActive(true);
            objectToDeactivate.SetActive(false);
        }
        else
        {
            StopAllCoroutines();
            transitionTimer = transitionTime;
            StartCoroutine(TransitionToActivationObject());
        }
    }

    private IEnumerator TransitionToActivationObject()
    {
        objectToDeactivate.SetActive(false);
        transitionObject.SetActive(true);
        // TODO play closing audio

        while (transitionTimer > 0)
        {
            transitionTimer -= Time.deltaTime;
            yield return null;
        }
        
        transitionObject.SetActive(false);
        objectToActivate.SetActive(true);
    }
}
