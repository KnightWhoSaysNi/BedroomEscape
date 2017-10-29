using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinKey : MonoBehaviour
{
    private void OnMouseDown()
    {
        AudioManager.Instance.PlayPuzzleSolvedAudio();
        Destroy(this.gameObject, 0.1f);
    }
}
