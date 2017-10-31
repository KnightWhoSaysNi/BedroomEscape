using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSender : MonoBehaviour
{
    [SerializeField] private string[] messages;
    [SerializeField] private bool shouldRepeatMessages;
    private int currentMessageIndex;

    private void Awake()
    {
        if (messages.Length == 0)
        {
            print("No messages yet on this message sender");
        }
    }

    private void OnMouseDown()
    {
        DialogManager.Instance.DisplayMessage(messages[currentMessageIndex]);

        if (shouldRepeatMessages)
        {
            currentMessageIndex = ++currentMessageIndex % messages.Length;
        }
        else if (currentMessageIndex != messages.Length - 1)
        {
            currentMessageIndex++;
        }
    }
}
