using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class HiddenObject : MonoBehaviour
{
    [SerializeField]
    public Sprite thumbnail;
    private ToyBox toyBox;   

    public void ConnectToyBox(ToyBox toyBox)
    {
        this.toyBox = toyBox;
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            toyBox.RegisterObjectSelection(this);            
        }
    }
}
