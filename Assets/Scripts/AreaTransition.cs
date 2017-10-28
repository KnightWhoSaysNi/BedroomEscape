using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AreaTransition : MonoBehaviour
{
    [SerializeField]
    private PuzzleArea puzzleArea;

    private void OnMouseDown()
    {
        GameManager.Instance.GoToPuzzleArea(puzzleArea);
    }
}
