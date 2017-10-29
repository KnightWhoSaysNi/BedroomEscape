using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class AreaTransition2D : MonoBehaviour
{
    [SerializeField]
    private PuzzleArea puzzleArea;

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            GameManager.Instance.GoToPuzzleArea(puzzleArea);
        }
    }
}
