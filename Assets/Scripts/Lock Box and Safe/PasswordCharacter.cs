using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordCharacter : MonoBehaviour
{
    [SerializeField] private Text displayText;
    [SerializeField] private PasswordType passwordType;
    [Space(10)]
    [SerializeField] private AudioClip characterChangeAudio;
    [SerializeField, Range(0f,100f)] private float volumePercent = 100;
    private char currentCharacter;
    private char startCharacter;
    private char endCharacter;

    public static event Action PasswordCharacterChanged;

    public char CurrentCharacter
    {
        get
        {
            return currentCharacter;
        }
        set
        {
            currentCharacter = value;
            if (PasswordCharacterChanged != null)
            {
                PasswordCharacterChanged();
            }
        }
    }

    /// <summary>
    /// Change the currently shown character either in positive or negative direction.
    /// </summary>
    public void ChangeCharacter(bool isPositiveChange)
    {
        AudioManager.Instance.PlayAudioClip(characterChangeAudio, volumePercent);

        if (isPositiveChange)
        {
            // Get next character
            if (CurrentCharacter == endCharacter)
            {
                CurrentCharacter = startCharacter;
            }
            else
            {
                CurrentCharacter++;
            }
        }
        else
        {
            // Get previous character
            if (CurrentCharacter == startCharacter)
            {
                CurrentCharacter = endCharacter;
            }
            else
            {
                CurrentCharacter--;
            }
        }

        displayText.text = CurrentCharacter.ToString();
    }

    private void Awake()
    {
        switch (passwordType)
        {
            case PasswordType.Alphabetic:
                startCharacter = 'A';
                endCharacter = 'Z';                
                break;
            case PasswordType.Numeric:
                startCharacter = '0';
                endCharacter = '9';
                break;
            default:
                throw new UnityException("No code for this password type yet!");
        }

        CurrentCharacter = startCharacter;
        displayText.text = CurrentCharacter.ToString();
    }    

    
}

public enum PasswordType { Alphabetic, Numeric }
