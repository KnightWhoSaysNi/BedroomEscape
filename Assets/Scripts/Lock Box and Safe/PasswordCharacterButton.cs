using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PasswordCharacterButton : MonoBehaviour
{
    [SerializeField] private PasswordCharacter passwordCharacter;
    [SerializeField] private bool isPositiveChange;

    private void OnMouseDown()
    {
        passwordCharacter.ChangeCharacter(isPositiveChange);
    }
}
