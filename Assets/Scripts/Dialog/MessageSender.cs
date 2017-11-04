using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class MessageSender : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string[] messages;    
    private int currentMessageIndex;
    private bool shouldShowMessages;

    public void DisableMessages()
    {
        shouldShowMessages = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (shouldShowMessages)
        {
            DialogManager.Instance.DisplayMessage(messages[currentMessageIndex]);
            currentMessageIndex = ++currentMessageIndex % messages.Length;
        }
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
}
