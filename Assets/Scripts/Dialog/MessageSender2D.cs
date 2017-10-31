using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class MessageSender2D : MonoBehaviour
{
    [SerializeField] private string[] messages;
    private int currentMessageIndex;

    private void Awake()
    {
        if (messages.Length == 0)
        {
            print("No messages yet on this message sender");
            return;
        }
    }

    private void OnMouseDown()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
        {
            DialogManager.Instance.DisplayMessage(messages[currentMessageIndex]);
            currentMessageIndex = ++currentMessageIndex % messages.Length;
        }
    }
}
