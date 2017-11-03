using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : MonoBehaviour
{
    [SerializeField] private PasswordPuzzle passwordPuzzle;
    [SerializeField] private MessageSender2D messageSender2D;
    [SerializeField] private PuzzleViewSwitcher switchablePuzzleView;

    private void Awake()
    {
        passwordPuzzle.PasswordPuzzleUpdate += OnPasswordPuzzleUpdate;
    }

    private void OnPasswordPuzzleUpdate(PasswordPuzzle updatedPasswordPuzzle, bool isPasswordCorrect)
    {
        if (this.passwordPuzzle == updatedPasswordPuzzle)
        {
            messageSender2D.enabled = !isPasswordCorrect;
            switchablePuzzleView.enabled = isPasswordCorrect;
        }
    }
}
