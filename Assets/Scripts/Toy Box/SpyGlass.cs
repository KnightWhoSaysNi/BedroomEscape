using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyGlass : MonoBehaviour
{
    [SerializeField] private float destroyDelaySeconds;

    private void Awake()
    {
        Destroy(this.gameObject, destroyDelaySeconds);
    }    
}
