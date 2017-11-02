using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : Manager
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private Text dialogBoxText;
    private float messageTime = 3;
    private float messageTimer;

    #region - "Singleton" Instance -
    private static DialogManager instance;

    public static DialogManager Instance
    {
        get
        {
            if (instance == null && !isApplicationClosing)
            {
                throw new UnityException("Someone is calling DialogManager.Instance before it is set! Change script execution order.");
            }

            return instance;
        }
    }

    private void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public void DisplayMessage(string message)
    {
        dialogBoxText.text = message;
        messageTimer = messageTime;

        if (!dialogBox.activeSelf)
        {
            dialogBox.SetActive(true);
        }
    }

    public void HideDialogBox()
    {
        messageTimer = 0;
        dialogBox.SetActive(false);
    }

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Update()
    {
        if (messageTimer > 0)
        {
            messageTimer -= Time.deltaTime;

            if (messageTimer <= 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }
}
