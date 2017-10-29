using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PuzzleItem : MonoBehaviour
{
    [SerializeField] private PuzzleItemType puzzleItemType;    

    private void OnMouseDown()
    {
        AudioManager.Instance.PlayPuzzleSolvedAudio();
        // TODO report to the manager that the object has been picked up
        Destroy(this.gameObject, 0.1f);
    }
}

public enum PuzzleItemType { BinKey, RedWireSpool, BlackWireSpool, WireSnips, EarthWire, Screwdriver, SafeKey }
