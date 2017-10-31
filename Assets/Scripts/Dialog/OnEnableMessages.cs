using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableMessages : MonoBehaviour
{
    [SerializeField] private string[] messages = new string[1] { string.Empty };
    [SerializeField] private bool shouldRepeatMessages;
    private int messageIndex;

    private bool areAllMessagesShown;

    private void OnEnable()
    {
        if (areAllMessagesShown && !shouldRepeatMessages)
        {
            // Messages should be shown only once and they were all diplayed already
            return;
        }

        DialogManager.Instance.DisplayMessage(messages[messageIndex]);

        if (messageIndex == messages.Length - 1)
        {
            areAllMessagesShown = true;
        }
        messageIndex = ++messageIndex % messages.Length;
    }
}
