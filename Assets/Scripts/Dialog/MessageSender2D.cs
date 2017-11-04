using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class MessageSender2D : MonoBehaviour
{
    [SerializeField] private string[] messages;
    private int currentMessageIndex;
    private bool shouldShowMessages;

    public void DisableMessages()
    {
        shouldShowMessages = false;
    }

    private void Awake()
    {
        if (messages.Length == 0)
        {
            print("No messages yet on this message sender");
            return;
        }

        shouldShowMessages = true;
    }

    private void OnMouseDown()
    {
        if(shouldShowMessages && !EventSystem.current.IsPointerOverGameObject())
        {
            DialogManager.Instance.DisplayMessage(messages[currentMessageIndex]);
            currentMessageIndex = ++currentMessageIndex % messages.Length;
        }
    }
}
