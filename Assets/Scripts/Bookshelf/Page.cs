using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class Page : MonoBehaviour
{
    public BookPage bookPage;

    public static event Action<BookPage> PageTurnRequest;

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (PageTurnRequest != null)
            {
                PageTurnRequest(bookPage);
            }
        }
    }
}

public enum BookPage { Left, Right }
