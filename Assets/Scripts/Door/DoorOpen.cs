using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoorOpen : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            GameManager.Instance.GoToEndGameScreen();
        }
    }
}
