using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [Space(5)]
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;
    private bool isSoundOn = true;
    
    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        buttonImage.sprite = isSoundOn ? soundOn : soundOff;
        AudioManager.Instance.ToggleSound();
    }
}
