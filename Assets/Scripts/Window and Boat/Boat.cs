using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField] private GameObject inventory;

    private void OnEnable()
    {
        inventory.SetActive(false);
    }

    private void OnDisable()
    {
        inventory.SetActive(true);
    }
}
