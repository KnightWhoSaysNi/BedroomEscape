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

    public void OnPointerClick(PointerEventData eventData)
    {
        DialogManager.Instance.DisplayMessage(messages[currentMessageIndex]);
        currentMessageIndex = ++currentMessageIndex % messages.Length;
    }

    private void Awake()
    {
        if (messages.Length == 0)
        {
            print("No messages yet on this message sender");
            return;
        }
    }    
}
