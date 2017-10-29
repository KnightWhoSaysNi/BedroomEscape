using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class AreaTransition : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private PuzzleArea puzzleArea;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.GoToPuzzleArea(puzzleArea);
    }
}
