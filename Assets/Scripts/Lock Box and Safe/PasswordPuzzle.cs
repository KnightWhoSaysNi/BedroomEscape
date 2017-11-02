using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PasswordPuzzle : MonoBehaviour
{
    [Tooltip("Password must contain only alphanumeric characters. (at the moment)")]
    [SerializeField] private string password;
    [SerializeField, Space(8)] private PasswordCharacter[] orderedPasswordCharacters;
    private bool isPasswordCorrect;

    public delegate void PasswordPuzzleUpdateDelegate(PasswordPuzzle passwordPuzzle, bool isPasswordCorrect);
    public event PasswordPuzzleUpdateDelegate PasswordPuzzleUpdate;

    public string Password
    {
        get
        {
            return password;
        }        
    }

    private void Awake()
    {
        PasswordCharacter.PasswordCharacterChanged += CheckIfPasswordFound;

        if (password.Length != orderedPasswordCharacters.Length)
        {
            throw new UnityException("Password has more characters than there are fields to input them in!");
        }

        password.ToUpper();
    }

    private void CheckIfPasswordFound()
    {
        isPasswordCorrect = true;

        for (int i = 0; i < password.Length; i++)
        {
            if (password[i] != orderedPasswordCharacters[i].CurrentCharacter)
            {
                isPasswordCorrect = false;
                break;
            }
        }

        if (PasswordPuzzleUpdate != null)
        {
            PasswordPuzzleUpdate(this, isPasswordCorrect);
        }
    }
}
